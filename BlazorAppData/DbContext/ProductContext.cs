

using Microsoft.EntityFrameworkCore;

namespace BlazorGrpc.Model;

public class ProductContext : DbContext
{
    public ProductContext(DbContextOptions<ProductContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(entity =>
        {

            entity.OwnsOne(p => p.Price, price =>
            {
                price.Property(p => p.Value)
                    .HasColumnName("Price")
                    .IsRequired();
            });
            entity.Navigation(x => x.Price).IsRequired();


            entity.OwnsOne(p => p.Name, name =>
            {
                name.Property(n => n.Value)
                    .HasColumnName("Name")
                    .IsRequired();
            });

            entity.Navigation(x => x.Name).IsRequired();

        });
    }
}