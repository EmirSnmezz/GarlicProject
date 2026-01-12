using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext (DbContextOptions options) : base (options) {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ModelAssembly).Assembly);
    }

    public override int SaveChanges()
    {
        var entries = ChangeTracker.Entries<IModel>();

        foreach(var entry in entries)
        {
            if(entry.State == EntityState.Added)
            {
                entry.Property(p => p.CreatedDate).CurrentValue = DateTime.UtcNow;
            }

            if(entry.State == EntityState.Modified)
            {
                entry.Property(p => p.UpdatedDate).CurrentValue = DateTime.UtcNow;
            }
        }

        return base.SaveChanges();
    }
}