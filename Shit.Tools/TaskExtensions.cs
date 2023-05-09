using Microsoft.Extensions.Logging;

namespace Shit.Tools;

public static class TaskExtensions
{
	/// <summary>
	///    Log all exceptions from all tasks passed into WhenAll
	/// </summary>
	/// <param name="aggregatedTask">The aggregated task returned from WhenAll</param>
	/// <param name="log">The logger used for Error logging of the exceptions for each failed task</param>
	public static async Task LogAllExceptions(this Task aggregatedTask, ILogger log)
	{
		try
		{
			await aggregatedTask;
		}
		catch (Exception)
		{
			foreach (var innerException in aggregatedTask.Exception.InnerExceptions)
				log.LogError(innerException, "Parallelized task failed.");
			throw;
		}
	}

	/// <summary>
	///    Log all exceptions from all tasks passed into WhenAll
	/// </summary>
	/// <param name="aggregatedTask">The aggregated task returned from WhenAll</param>
	/// <param name="log">The logger used for Error logging of the exceptions for each failed task</param>
	/// <typeparam name="TResult">The type of the task results</typeparam>
	/// <returns>An IEnumerable with results from each Task</returns>
	public static async Task<IEnumerable<TResult>> LogAllExceptions<TResult>(this Task<TResult[]> aggregatedTask, ILogger log)
	{
		try
		{
			return await aggregatedTask;
		}
		catch (Exception)
		{
			foreach (var innerException in aggregatedTask.Exception.InnerExceptions)
				log.LogError(innerException, "Parallelized task failed.");
			throw;
		}
	}

	/// <summary>
	///    Combine IEnumerable results from all tasks and log any exceptions from the WhenAll tasks.
	/// </summary>
	/// <param name="tasks">An array of Tasks with IEnumerable results</param>
	/// <param name="log">The logger used for Error logging of the exceptions for each failed task</param>
	/// <typeparam name="TResult">The type of the Task results</typeparam>
	/// <returns>A single combined IEnumerable with results from each Task</returns>
	public static async Task<IEnumerable<TResult>> LogAllExceptions<TResult>(this Task<IEnumerable<TResult>[]> tasks, ILogger log)
	{
		try
		{
			return (await tasks).SelectMany(x => x);
		}
		catch (Exception)
		{
			foreach (var innerException in tasks.Exception.InnerExceptions)
				log.LogError(innerException, "Parallelized task failed.");
			throw;
		}
	}
}
