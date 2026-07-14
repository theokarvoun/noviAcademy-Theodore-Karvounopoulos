namespace WorldRank.Infrastructure.Persistence;

/// <summary>
/// Single place for the connection string used by the DbContext, the migration factory and
/// the Dapper MERGE (<see cref="WalletBulkUpsert"/>). Points at the local SQL Server.
/// NOTE: this is a local dev connection — move it to user-secrets / appsettings / an
/// environment variable before using anything like this outside the lab.
/// </summary>
public static class DbConnection
{
	public const string ConnectionString =
		"Server=localhost;Database=WorldRank;Integrated Security=true;TrustServerCertificate=true";
}
