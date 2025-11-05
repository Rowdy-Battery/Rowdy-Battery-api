using Microsoft.EntityFrameworkCore;
using RowdyBattery.Domain.Catalog;

namespace RowdyBattery.Data;

public class StoreContext : DbContext
{
    public StoreContext() { }
    public StoreContext(DbContextOptions<StoreContext> options) : base(options) { }

    public DbSet<Item> Items => Set<Item>();
    public DbSet<Rating> Ratings => Set<Rating>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Simple seeding for Items (Ratings optional for now)
        modelBuilder.Entity<Item>().HasData(
            new Item(1, "Rowdy Battery 1000mAh", 49.99m),
            new Item(2, "Rowdy Battery 5000mAh", 79.99m),
            new Item(3, "Rowdy Battery 10000mAh", 119.99m)
        );
    }
}
