using System.Transactions;

namespace Shit.Tools;

public static class TransactionHelper
{
	public static TransactionScope AsyncTransactionScope(IsolationLevel? isolationLevel = null, TimeSpan? timeout = null)
	{
		var options = new TransactionOptions
		{
			IsolationLevel = isolationLevel ?? IsolationLevel.Serializable, // ReadCommitted? Chaos?
			Timeout = timeout ?? TransactionManager.DefaultTimeout // MaximumTimeout?
		};
		var scope = new TransactionScope(TransactionScopeOption.Required, options, TransactionScopeAsyncFlowOption.Enabled);

		TransactionManager.ImplicitDistributedTransactions = true;
		TransactionInterop.GetTransmitterPropagationToken(Transaction.Current);
		return scope;
	}

	public static TransactionScope SuppressTransactionScope() => new(TransactionScopeOption.Suppress, TransactionScopeAsyncFlowOption.Enabled);
}
