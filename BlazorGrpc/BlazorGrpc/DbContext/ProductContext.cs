using BlazorGrpc.Model;

using Microsoft.EntityFrameworkCore;

namespace BlazorGrpc.Model;

public class ProductContext : DbContext
{
    public ProductContext(DbContextOptions<ProductContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
}