using Dapper;
using Microsoft.Data.SqlClient;
using WorldRank.Domain.Entities;

namespace WorldRank.Infrastructure.Persistence;

/// <summary>
/// Raw SQL MERGE via Dapper — upsert a batch of wallets in a single transaction
/// (match on Id: update the balance/blocked flag if the row exists, insert it otherwise).
/// Adapted to WorldRank's integer-keyed <see cref="Wallet"/> and the "Wallets" table schema
/// defined in <c>WorldRankDBContext</c> (Currency stored as its readable code).
/// </summary>
public class WalletBulkUpsert
{
	private readonly string _connectionString;

	public WalletBulkUpsert(string connectionString) => _connectionString = connectionString;

	public async Task UpsertAsync(IEnumerable<Wallet> wallets)
	{
		const string sql = @"
MERGE INTO Wallets AS target
USING (VALUES (@Id, @PlayerId, @Currency, @Balance, @IsBlocked))
    AS source (Id, PlayerId, Currency, Balance, IsBlocked)
ON target.Id = source.Id
WHEN MATCHED THEN
    UPDATE SET Balance = source.Balance, IsBlocked = source.IsBlocked
WHEN NOT MATCHED THEN
    INSERT (Id, PlayerId, Currency, Balance, IsBlocked)
    VALUES (source.Id, source.PlayerId, source.Currency, source.Balance, source.IsBlocked);";

		var rows = wallets.Select(wallet => new
		{
			wallet.Id,
			wallet.PlayerId,
			Currency = wallet.Currency.ToString(),
			wallet.Balance,
			wallet.IsBlocked
		});

		await using var connection = new SqlConnection(_connectionString);
		await connection.OpenAsync();
		await using var transaction = await connection.BeginTransactionAsync();

		// The whole batch commits (or rolls back) as one transaction.
		await connection.ExecuteAsync(sql, rows, transaction);
		await transaction.CommitAsync();
	}
}
