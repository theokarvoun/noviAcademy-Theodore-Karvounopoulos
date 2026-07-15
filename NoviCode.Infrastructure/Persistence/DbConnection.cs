namespace NoviCode;

// Single place for the connection string used by the DbContext and the Dapper MERGE.
// Points at the local SQL Server running in Docker (container `ms-sql`, port 1433).
// NOTE: this is a local dev credential — move it to user-secrets / appsettings / an
// environment variable before using anything like this outside the lab.
public static class DbConnection
{
	// Use LocalDB for local development to avoid depending on an external SQL Server instance.
	public const string ConnectionString = "Server=(localdb)\\MSSQLLocalDB;Database=NoviCode;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=true";
}
