using Serilog;
using Serilog.Core;
using Serilog.Events;
using System.Transactions;

namespace Shit.Tools;

public class TransactionEnricher : ILogEventEnricher
{
	public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
	{
		var transaction = Transaction.Current;
		if (transaction == null) return;
		logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("TransactionStatus", transaction.TransactionInformation.Status));
		logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("TransactionLocalId", transaction.TransactionInformation.LocalIdentifier));
		logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("TransactionDistributedId", transaction.TransactionInformation.DistributedIdentifier));
		logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("TransactionPromoterType", transaction.PromoterType));
		logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("TransactionCreationTime", transaction.TransactionInformation.CreationTime));
		logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("TransactionIsolationLevel", transaction.IsolationLevel));
	}
}
