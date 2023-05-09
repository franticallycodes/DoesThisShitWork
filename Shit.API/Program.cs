using Lamar.Microsoft.DependencyInjection;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;
using Shit.Data;
using Shit.Tools;
using Shit.API.Registries;
using Shit.Service;

Log.Logger = new LoggerConfiguration()
	.MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
	.MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Warning)
	.Enrich.FromLogContext()
	.Enrich.WithProcessId()
	.Enrich.WithThreadId()
	.Enrich.With(new TransactionEnricher())
	.WriteTo.Console(new RenderedCompactJsonFormatter())
	.WriteTo.File("./LogFiles/TxScope-.log",
		rollingInterval: RollingInterval.Day,
		outputTemplate:
		@"{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] {ThreadId:D3} {SourceContext:l} {Message:lj}
		TransactionStatus: {TransactionStatus}
		TransactionLocalId: {TransactionLocalId}
		TransactionDistributedId: {TransactionDistributedId}
		TransactionPromoterType: {TransactionPromoterType}
		TransactionCreationTime: {TransactionCreationTime}
		TransactionIsolationLevel: {TransactionIsolationLevel}
		{Exception}")
	.CreateLogger();

try
{
	var builder = WebApplication.CreateBuilder(args);

	// Add services to the container.
	builder.Host.UseLamar((context, registry) =>
	{
		registry.AddLogging();
		registry.For<IStall>().Use<Stall>();
		registry.For<IPoop>().Use<Poop>();

		// Add your own Lamar ServiceRegistry collections
		// of registrations
		registry.IncludeRegistry<DatabaseConnectionRegistry>();

		// discover MVC controllers -- this was problematic
		// inside of the UseLamar() method, but is "fixed" in
		// Lamar V8
		registry.AddControllers();

		// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
		registry.AddEndpointsApiExplorer();
		registry.AddSwaggerGen();
	});

	builder.Host.UseSerilog();

	var app = builder.Build();
	app.UseSerilogRequestLogging(options =>
	{
		// Customize the message template
		options.MessageTemplate = "API Handled {RequestPath}";

		// Emit debug-level events instead of the defaults
		options.GetLevel = (httpContext, elapsed, ex) => LogEventLevel.Warning;

		// Attach additional properties to the request completion event
		options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
		{
			diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
			diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme);
		};
	});

// Configure the HTTP request pipeline.
	if (app.Environment.IsDevelopment())
	{
		app.UseSwagger();
		app.UseSwaggerUI();
	}

	app.UseHttpsRedirection();

	app.UseAuthorization();

	app.MapControllers();

	app.Run();
}
catch (Exception exception)
{
	Log.Fatal(exception, "Application terminated unexpectedly");
	throw;
}
finally
{
	Log.CloseAndFlush();
}
