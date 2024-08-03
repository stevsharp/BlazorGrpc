

using BlazorGrpc.Model;

namespace BlazorAppData.Interrface;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetProductsAsync();

    Task<Product> GetProductByIdAsync(object id);

    Task<bool> DeleteProduct(Product product);

    Task<bool> UpdateProduct(Product product);

    Task<bool> CreateProdut(Product product);

}
