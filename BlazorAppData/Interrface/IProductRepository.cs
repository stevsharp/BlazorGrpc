

using BlazorGrpc.Model;

namespace BlazorAppData.Interrface;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetProductsAsync();

    Task<Product> GetProductByIdAsync(object id);
    Task DeleteProductAsync(Product product);

    Task DeleteProductAsync(int productId);

    Task UpdateProductAsync(Product product);

    Task CreateProdutAsync(Product product);

}
