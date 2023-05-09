using System.Transactions;
using Microsoft.AspNetCore.Mvc;
using Shit.Service;
using Shit.Tools;

namespace Shit.API.Controllers;

[ApiController]
[Route("tx")]
public class TransactionTestController : ControllerBase
{
	readonly ILogger<TransactionTestController> _logger;
	readonly IStall _stall;

	public TransactionTestController(ILogger<TransactionTestController> logger, IStall stall)
	{
		_logger = logger;
		_stall = stall;
	}

	[HttpGet("async/scope/working")]
	public async Task<IActionResult> WithTransactionScopeAsyncEnabled()
	{
		using var timer = ShitTimer();
		using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
		await _stall.PoopAsync();
		scope.Complete();FlushThatLog();
		return Ok();
	}

	[HttpGet("async/scope/notworking")]
	public async Task<IActionResult> WithTransactionScopeAsyncDisabled()
	{
		using var timer = ShitTimer();
		using var scope = new TransactionScope();
		await _stall.PoopAsync();
		scope.Complete();
		FlushThatLog();
		return Ok();
	}

	[HttpGet("sync/scope/working")]
	public IActionResult WithTransactionScope()
	{
		using var timer = ShitTimer();
		using var scope = new TransactionScope();
		_stall.Poop();
		scope.Complete();
		FlushThatLog();
		return Ok();
	}

	LogTimer ShitTimer() => new(_logger, "let's get ready to rumble 🚽");
	void FlushThatLog() => _logger.LogInformation("feels good man 💩");
}
