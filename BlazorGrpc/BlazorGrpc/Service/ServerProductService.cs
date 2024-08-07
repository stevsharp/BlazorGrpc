using BlazorAppData.Interrface;

using BlazorGrpc.gRPC;
using BlazorGrpc.Handler;
using BlazorGrpcSimpleMediater;
using Grpc.Core;

namespace BlazorGrpc.Service;

public class ServerProductService : ProductService.ProductServiceBase
{
    private readonly IMediator _mediator;
    public ServerProductService(IMediator mediator) => _mediator = mediator;

    public override async Task<ProductResponse> GetProduct(GetProductRequest request, ServerCallContext context)
    {
       var response = await _mediator.Send(new GetProductByIdCommand(request.Id)).ConfigureAwait(false);

        return new ProductResponse
        {
            Id = response.Id,
            Name = response.Name,
            Price = (float)response.Price
        };
    }

    public override async Task<ProductResponse> CreateProduct(CreateProductRequest request, ServerCallContext context)
    {
        var response = await _mediator.Send(new CreateProductCommand(request.Name, (decimal)request.Price)).ConfigureAwait(false);

        return new ProductResponse
        {
            Id = response.Id,
            Name = response.Name,
            Price = (float)response.Price
        };
    }

    public override async Task<ProductResponse> UpdateProduct(UpdateProductRequest request, ServerCallContext context)
    {
        var response = await _mediator.Send(new UpdateProductCommand(request.Id, request.Name, (decimal)request.Price)).ConfigureAwait(false);

        return new ProductResponse
        {
            Id = response.Id,
            Name = response.Name,
            Price = (float)response.Price
        };
    }

    public override async Task<Empty> DeleteProduct(DeleteProductRequest request, ServerCallContext context)
    {

        var response = await _mediator.Send(new DeleteProductByIdCommand(request.Id)).ConfigureAwait(false);

        if (!response)
            throw new RpcException(new Status(StatusCode.NotFound, "Product not found"));

        return new Empty();
    }

    public override async Task<ProductListResponse> ListProducts(Empty request, ServerCallContext context)
    {
        var response = await _mediator.Send(new GetAllProductCommand()).ConfigureAwait(false);

        var productListResponse = new ProductListResponse();

        productListResponse.Products.AddRange(response.Select(x => new ProductResponse
        {
            Id = x.Id,
            Name = x.Name,
            Price = (float)x.Price
        }));

        return productListResponse;
    }
}
