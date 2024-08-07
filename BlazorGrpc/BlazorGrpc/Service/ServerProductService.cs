using BlazorAppData.Interrface;

using BlazorGrpc.gRPC;
using BlazorGrpc.Handler;
using BlazorGrpc.Model;

using BlazorGrpcSimpleMediater;

using Grpc.Core;

namespace BlazorGrpc.Service;


public class ServerProductService : ProductService.ProductServiceBase
{
    private readonly IProductRepository _productRepository;

    private readonly IMediator _mediator;
    public ServerProductService(IProductRepository productRepository, IMediator mediator)
    {
        _productRepository = productRepository;

        _mediator = mediator;
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

        var response = await _mediator.Send(new CreateProductCommand(request.Name, (decimal)request.Price));

        return new ProductResponse
        {
            Id = response.Id,
            Name = response.Name,
            Price = (float)response.Price
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

        var isUpdated = await _productRepository.UpdateProduct(product);

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

        response.Products.AddRange(products.Select(x => new ProductResponse
        {
            Id = x.Id,
            Name = x.Name,
            Price = (float)x.Price
        }));

        return response;
    }
}
