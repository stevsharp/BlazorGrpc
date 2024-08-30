

using BlazorGrpc.Model;

namespace BlazorAppData.Interrface;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetProductsAsync();

    Task<Product> GetProductByIdAsync(object id);
    Task DeleteProduct(Product product);

    Task UpdateProduct(Product product);

    Task CreateProdut(Product product);

}
