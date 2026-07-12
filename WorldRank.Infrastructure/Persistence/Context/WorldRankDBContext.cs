using Microsoft.EntityFrameworkCore;
using WorldRank.Domain.Entities;

namespace WorldRank.Infrastructure.Persistence.Context
{
    public partial class WorldRankDBContext : DbContext
    {
        public DbSet<Player> Players { get; set; } = null!;
        public DbSet<Wallet> Wallets { get; set; } = null!;

        public WorldRankDBContext(DbContextOptions<WorldRankDBContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Player>(entity =>
            {
                entity.ToTable("Players");

                entity.HasKey(player => player.Id);

                // The application assigns the Id explicitly, so the database must not generate it.
                entity.Property(player => player.Id).ValueGeneratedNever();

                entity.Property(player => player.Name).HasMaxLength(100).IsUnicode(false).IsRequired();
                entity.Property(player => player.Score).IsRequired();
            });

            modelBuilder.Entity<Wallet>(entity =>
            {
                entity.ToTable("Wallets");

                // Id is the primary key (assigned by the app, same approach as Player).
                entity.HasKey(wallet => wallet.Id);
                entity.Property(wallet => wallet.Id).ValueGeneratedNever();

                entity.Property(wallet => wallet.PlayerId).IsRequired();

                // Store the currency as its readable code (EUR, USD, ...) instead of a raw int.
                entity.Property(wallet => wallet.Currency)
                    .HasConversion<string>()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .IsRequired();

                entity.Property(wallet => wallet.Balance)
                    .HasPrecision(18, 2)
                    .IsRequired();

                entity.Property(wallet => wallet.IsBlocked).IsRequired();

                // Domain rule: a player can hold at most one wallet per currency.
                entity.HasIndex(wallet => new { wallet.PlayerId, wallet.Currency }, "IX_Wallets_PlayerId_Currency").IsUnique();

                // A wallet belongs to a player; deleting a player removes their wallets.
                entity.HasOne<Player>()
                    .WithMany()
                    .HasForeignKey(wallet => wallet.PlayerId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Wallets_Players");
            });
        }
    }
}
