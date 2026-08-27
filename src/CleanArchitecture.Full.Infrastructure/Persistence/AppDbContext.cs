using CleanArchitecture.Full.Domain;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Full.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Client> Clients => Set<Client>();
    public DbSet<Account> Accounts => Set<Account>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure Client entity
        modelBuilder.Entity<Client>(entity =>
        {
            entity.ToTable("clients");
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Name).IsRequired().HasMaxLength(200);
            entity.Property(c => c.Email).IsRequired().HasMaxLength(100);
            entity.Property(c => c.PhoneNumber).IsRequired().HasMaxLength(20);
            entity.Property(c => c.DocumentNumber).IsRequired().HasMaxLength(20);
            entity.Property(c => c.DocumentType).IsRequired().HasMaxLength(10);
            entity.Property(c => c.CreatedAt).IsRequired();
            entity.Property(c => c.LastModifiedAt);
            entity.HasIndex(c => c.Email).IsUnique();
            entity.HasIndex(c => c.DocumentNumber).IsUnique();
        });

        // Configure Account entity with relationship to Client
        modelBuilder.Entity<Account>(entity =>
        {
            entity.ToTable("accounts");
            entity.HasKey(a => a.Id);
            entity.Property(a => a.ClientId).IsRequired();
            entity.Property(a => a.AccountNumber).IsRequired().HasMaxLength(20);
            entity.Property(a => a.HolderName).IsRequired().HasMaxLength(150);
            entity.Property(a => a.Balance).HasColumnType("numeric(18,2)");
            entity.Property(a => a.Status).IsRequired().HasMaxLength(20);
            entity.Property(a => a.AccountType).IsRequired().HasMaxLength(50);
            entity.Property(a => a.OpenedAt).IsRequired();
            entity.Property(a => a.LastModifiedAt);
            entity.HasIndex(a => a.AccountNumber).IsUnique();

            // Configure One-to-Many relationship
            entity.HasOne(a => a.Client)
                .WithMany(c => c.Accounts)
                .HasForeignKey(a => a.ClientId)
                .OnDelete(DeleteBehavior.Cascade);

            // Always include the related Client when querying Accounts
            // This makes EF automatically eager-load the Client navigation so it won't be null
            entity.Navigation(a => a.Client).AutoInclude();
        });
    }
}
