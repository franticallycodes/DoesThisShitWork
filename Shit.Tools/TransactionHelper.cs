using System.Transactions;

namespace Shit.Tools;

public static class TransactionHelper
{
	public static TransactionScope AsyncTransactionScope(IsolationLevel? isolationLevel = null, TimeSpan? timeout = null)
	{
		var options = new TransactionOptions
		{
			IsolationLevel = isolationLevel ?? IsolationLevel.Serializable,
			Timeout = timeout ?? TransactionManager.DefaultTimeout
		};
		var scope = new TransactionScope(TransactionScopeOption.RequiresNew, options, TransactionScopeAsyncFlowOption.Enabled);

//		TransactionManager.ImplicitDistributedTransactions = true;
//		TransactionInterop.GetTransmitterPropagationToken(Transaction.Current);
		return scope;
	}

	public static TransactionScope SuppressTransactionScope() => new(TransactionScopeOption.Suppress, TransactionScopeAsyncFlowOption.Enabled);
}
