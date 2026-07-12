using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace WorldRank.Infrastructure.Persistence.Context
{
    /// <summary>
    /// Design-time factory used by the EF Core tools (Add-Migration / Update-Database).
    /// The tools call this to build a <see cref="WorldRankDBContext"/> without running the app,
    /// so the connection string here is only used at design time.
    /// </summary>
    public class WorldRankDBContextFactory : IDesignTimeDbContextFactory<WorldRankDBContext>
    {
        public WorldRankDBContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<WorldRankDBContext>();
            optionsBuilder.UseSqlServer("Server=localhost;Database=WorldRank;Integrated Security=true;TrustServerCertificate=true");

            return new WorldRankDBContext(optionsBuilder.Options);
        }
    }
}
