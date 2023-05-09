using Microsoft.Extensions.Logging;
using Shit.Data;
using Shit.Tools;

namespace Shit.Service;

public class Stall : IStall
{
	readonly ILogger<Stall> _logger;
	readonly IPoop _poop;

	public Stall(ILogger<Stall> logger, IPoop poop)
	{
		_logger = logger;
		_poop = poop;
	}

	public async Task PoopAsync(int pooperCount = 10)
	{
		_logger.LogWarning("shits coming");

		var fullStalls = Poopers.Dudes.Take(pooperCount).Select(_poop.GoPottyAsync);

		await Task.WhenAll(fullStalls).LogAllExceptions(_logger);

		_logger.LogWarning("everyone is about to flush");
	}

	public void Poop(int pooperCount = 10)
	{
		_logger.LogWarning("shits coming");

		foreach (var dude in Poopers.Dudes.Take(pooperCount))
		{
			_poop.GoPotty(dude);
		}

		_logger.LogWarning("everyone is about to flush");
	}
}

public interface IStall
{
	Task PoopAsync(int pooperCount = 10);
	void Poop(int pooperCount = 10);
}
