using System.Data;
using Dapper;
using Microsoft.Extensions.Logging;

namespace Shit.Data;

public class Poop : IPoop
{
	readonly IDbConnection _dbConnection;
	readonly ILogger<Poop> _logger;

	public Poop(IDbConnection dbConnection, ILogger<Poop> logger)
	{
		_dbConnection = dbConnection;
		_logger = logger;
	}

	public async Task GoPottyAsync(string personName)
	{
		try
		{
			var sql = $"print '{personName} is currently farting'";
			_logger.LogWarning(sql);
			await _dbConnection.ExecuteAsync(sql);
			Thread.Sleep(1000);
			_logger.LogWarning("dude that shit smells {PersonName}", personName);
		}
		catch (Exception exception)
		{
			_logger.LogError(exception, "{PersonName} shit their pants", personName);
			throw;
		}
	}

	public void GoPotty(string personName)
	{
		try
		{
			var sql = $"print '{personName} is currently farting'";
			_logger.LogWarning(sql);
			_dbConnection.Execute(sql);
		}
		catch (Exception exception)
		{
			_logger.LogError(exception, "Looks like {personName} shit themselves", personName);
			throw;
		}
	}
}

public interface IPoop
{
	Task GoPottyAsync(string personName);
	void GoPotty(string personName);
}
