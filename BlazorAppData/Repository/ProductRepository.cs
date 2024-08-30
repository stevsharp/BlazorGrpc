

using BlazorAppData.Interrface;

using BlazorGrpc.Model;

using Microsoft.EntityFrameworkCore;

namespace BlazorAppData.Repository;

public class ProductRepository : IProductRepository
{
    private readonly ProductContext _context;

    public ProductRepository(ProductContext context)
    {
        _context = context;
    }
    public async Task CreateProdut(Product product)
    {
        ArgumentNullException.ThrowIfNull(product);

        var productForCreate = new Product
        {
            Name = product.Name,
            Price = product.Price
        };

        await _context.Products.AddAsync(product);
    }

    public async Task DeleteProduct(Product product)
    {
        ArgumentNullException.ThrowIfNull(product);

        var id = product.Id;

        var productForDelete = await _context.Products.FindAsync(id) ?? throw new ArgumentException($"product with Id : {product.Id} not found !!!");

        _context.Products.Remove(productForDelete);

    }

    public async Task<Product> GetProductByIdAsync(object id)
    {
        var product = await _context.Products.FindAsync(id);

        return product ?? new Product();
    }

    public async Task<IEnumerable<Product>> GetProductsAsync()
    {
       return await this._context.Products.ToListAsync();
    }

    public async Task UpdateProduct(Product product)
    {
        ArgumentNullException.ThrowIfNull(product);

        var id = product.Id;

        var productForUpdate = await _context.Products.FindAsync(id) ?? throw new ArgumentException($"product with Id : {product.Id} not found !!!");

        productForUpdate.Name = product.Name;

        productForUpdate.Price = product.Price;

        _context.Products.Update(productForUpdate);

    }
}
