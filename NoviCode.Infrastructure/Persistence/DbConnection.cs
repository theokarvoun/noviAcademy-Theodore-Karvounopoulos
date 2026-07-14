namespace NoviCode;

// Single place for the connection string used by the DbContext and the Dapper MERGE.
// Points at the local SQL Server running in Docker (container `ms-sql`, port 1433).
// NOTE: this is a local dev credential — move it to user-secrets / appsettings / an
// environment variable before using anything like this outside the lab.
public static class DbConnection
{
    public const string ConnectionString =
        // Explicitly enable encryption and trust the server certificate for local/dev
        // environments to avoid SSL validation errors when using self-signed certs
        // (Keep Trusted_Connection=True for local Windows auth). Remove TrustServerCertificate
        // or set to false in production and use a certificate from a trusted CA.
        "Server=localhost;Database=WorldRank;Trusted_Connection=True;Encrypt=True;TrustServerCertificate=True;";
}
