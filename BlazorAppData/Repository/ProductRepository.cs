

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
    public async Task CreateProdutAsync(Product product)
    {
        ArgumentNullException.ThrowIfNull(product);

        var productForCreate = new Product
        {
            Name = product.Name,
            Price = product.Price
        };

        await _context.Products.AddAsync(product);
    }

    public async Task DeleteProductAsync(Product product)
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
       return await this._context
                .Products
                .AsNoTracking()
                .ToListAsync();
    }

    public async Task UpdateProductAsync(Product product)
    {
        ArgumentNullException.ThrowIfNull(product);

        var id = product.Id;

        await _context.Products.Where(x => x.Id == id)
            .ExecuteUpdateAsync(setters => setters.SetProperty(x => x.Name ,product.Name)
            .SetProperty(x => x.Price, product.Price));


    }
}


//var productForUpdate = await _context.Products.FindAsync(id) ?? throw new ArgumentException($"product with Id : {product.Id} not found !!!");

//productForUpdate.Name = product.Name;

//productForUpdate.Price = product.Price;

//_context.Products.Update(productForUpdate);
