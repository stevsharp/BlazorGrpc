

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
    public async Task<bool> CreateProdut(Product product)
    {
        ArgumentNullException.ThrowIfNull(product);

        var productForCreate = new Product
        {
            Name = product.Name,
            Price = product.Price
        };

        _context.Products.Add(product);

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteProduct(Product product)
    {
        ArgumentNullException.ThrowIfNull(product);

        var id = product.Id;

        var productForDelete = await _context.Products.FindAsync(id);

        if (product is not null) {

             _context.Products.Remove(productForDelete);

            return (await _context.SaveChangesAsync()) > 0;
        }

        return false;
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

    public async Task<bool> UpdateProduct(Product product)
    {
        ArgumentNullException.ThrowIfNull(product);

        var id = product.Id;

        var productForUpdate = await _context.Products.FindAsync(id);

        if (productForUpdate is not null)
        {

            productForUpdate.Name = product.Name;

            productForUpdate.Price = product.Price;

            _context.Products.Update(productForUpdate);

            return (await _context.SaveChangesAsync()) > 0;
        }

        return false;
    }
}
