using Mgtt.ECom.Domain.OrderManagement;
using Mgtt.ECom.Domain.ProductManagement;
using Mgtt.ECom.Domain.ReviewManagement;
using Mgtt.ECom.Domain.ShoppingCart;
using Mgtt.ECom.Domain.UserManagement;
using Microsoft.EntityFrameworkCore;

namespace Mgtt.ECom.Persistence.DataAccess;
/// <summary>
/// Represents the PostgreSQL database context
/// </summary>
public class PsqlDbContext : DbContext
{
    /// <summary>Default constructor<\summary>
    /// <remark>
    /// Comment this line out when initially creating first migration with 
    /// `dotnet-ef migrations add InitialCreate --output-dir Migrations`
    /// <\remark>
    public PsqlDbContext(DbContextOptions<PsqlDbContext> options) : base(options)
    { }

    public DbSet<User> Users { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<CartItem> CartItems { get; set; }

    /// <summary>
    /// Configures the database connection and options.
    /// </summary>
    /// <param name="optionsBuilder">Options builder for configuring the DbContext.</param>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseNpgsql();
    }

    /// <summary>
    /// Configures the entity mappings and relationships.
    /// </summary>
    /// <param name="modelBuilder">Model builder for building entity mappings.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(e =>
        {
            e.HasKey(o => o.UserID);
            e.Property(o => o.UserID).ValueGeneratedOnAdd();
        });
        modelBuilder.Entity<Order>(e =>
        {
            e.HasKey(o => o.OrderID);
            e.Property(o => o.OrderID).ValueGeneratedOnAdd();
        });
        modelBuilder.Entity<OrderItem>(e =>
        {
            e.HasKey(o => o.OrderItemID);
            e.Property(o => o.OrderItemID).ValueGeneratedOnAdd();
        });
        modelBuilder.Entity<Product>(e =>
        {
            e.HasKey(o => o.ProductID);
            e.Property(o => o.ProductID).ValueGeneratedOnAdd();
        });
        modelBuilder.Entity<Category>(e =>
        {
            e.HasKey(o => o.CategoryID);
            e.Property(o => o.CategoryID).ValueGeneratedOnAdd();
        });
        modelBuilder.Entity<Review>(e =>
        {
            e.HasKey(o => o.ReviewID);
            e.Property(o => o.ReviewID).ValueGeneratedOnAdd();
        });
        modelBuilder.Entity<Cart>(e =>
        {
            e.HasKey(o => o.CartID);
            e.Property(o => o.CartID).ValueGeneratedOnAdd();
        });
        modelBuilder.Entity<CartItem>(e =>
        {
            e.HasKey(o => o.CartItemID);
            e.Property(o => o.CartItemID).ValueGeneratedOnAdd();
        });
    }
}