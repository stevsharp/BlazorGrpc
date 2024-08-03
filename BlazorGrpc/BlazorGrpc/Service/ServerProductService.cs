using BlazorAppData.Interrface;

using BlazorGrpc.gRPC;
using BlazorGrpc.Model;

using Grpc.Core;

namespace BlazorGrpc.Service;


public class ServerProductService : ProductService.ProductServiceBase
{
    private readonly IProductRepository _productRepository;
    public ServerProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public override async Task<ProductResponse> GetProduct(GetProductRequest request, ServerCallContext context)
    {
        var product = await _productRepository.GetProductByIdAsync(request.Id);

        if (product == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, "Product not found"));
        }

        return new ProductResponse
        {
            Id = product.Id,
            Name = product.Name,
            Price = (float)product.Price
        };
    }

    public override async Task<ProductResponse> CreateProduct(CreateProductRequest request, ServerCallContext context)
    {
        var product = new Product
        {
            Name = request.Name,
            Price = (decimal)request.Price
        };

        var isCreated = await _productRepository.CreateProdut(product); 

        if (!isCreated)
            throw new RpcException(new Status(StatusCode.NotFound, "Product Could not be Created !!!!"));

        return new ProductResponse
        {
            Id = product.Id,
            Name = product.Name,
            Price = (float)product.Price
        };
    }

    public override async Task<ProductResponse> UpdateProduct(UpdateProductRequest request, ServerCallContext context)
    {
        var product = new Product
        {
            Id = request.Id,
            Name = request.Name,
            Price = (decimal)request.Price
        };

        var isUpdated =  await _productRepository.UpdateProduct(product);

        if (!isUpdated)
            throw new RpcException(new Status(StatusCode.NotFound, "Product Could not be Updated !!!"));

        return new ProductResponse
        {
            Id = product.Id,
            Name = product.Name,
            Price = (float)product.Price
        };
    }

    public override async Task<Empty> DeleteProduct(DeleteProductRequest request, ServerCallContext context)
    {

        var product = new Product
        {
            Id = request.Id
        };

        var isDeleted = await _productRepository.DeleteProduct(product);

        if (!isDeleted)
            throw new RpcException(new Status(StatusCode.NotFound, "Product not found"));

        return new Empty();
    }

    public override async Task<ProductListResponse> ListProducts(Empty request, ServerCallContext context)
    {
        var products = await _productRepository.GetProductsAsync() ?? Enumerable.Empty<Product>();

        var response = new ProductListResponse();

        response.Products.AddRange(products.Select(x=> new ProductResponse
        {
            Id = x.Id,
            Name = x.Name,
            Price = (float)x.Price
        }));

        return response;
    }
}
