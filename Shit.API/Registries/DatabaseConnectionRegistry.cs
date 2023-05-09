using System.Data;
using System.Data.SqlClient;
using Lamar;

namespace Shit.API.Registries;

public class DatabaseConnectionRegistry : ServiceRegistry
{
	public DatabaseConnectionRegistry()
	{
		For<IDbConnection>().Use(_ =>
		{
			var sqlConnection = new SqlConnection("Data Source=(local);Initial Catalog=ERM365;Integrated Security=SSPI;MultipleActiveResultSets=true");
			sqlConnection.Open();
			return sqlConnection;
		}).Scoped();
	}
}
