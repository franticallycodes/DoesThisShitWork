using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace Shit.Tools;

public class LogTimer : Stopwatch, IDisposable
{
	readonly ILogger _log;
	readonly string _message;
	readonly object[] _propertyValues;

	public LogTimer(ILogger log, string message, params object[] propertyValues)
	{
		_log = log;
		_message = message;
		_propertyValues = propertyValues;
		StartNew();
	}

	/// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
	public void Dispose()
	{
		Stop();
		_log.LogInformation($"{_message} ended after {Elapsed}", _propertyValues);
	}

	public void Clock(string action, params object[] propertyValues) =>
		_log.LogDebug($"{action} clocked in @ {Elapsed}", propertyValues);
}
