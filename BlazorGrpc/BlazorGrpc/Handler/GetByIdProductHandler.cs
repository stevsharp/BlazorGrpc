using BlazorAppData.Interrface;
using BlazorGrpcSimpleMediater;
using Grpc.Core;

namespace BlazorGrpc.Handler;

public class GetProductByIdCommand : IRequest<GetByIdProductResponse>
{
    public int Id { get; }

    public GetProductByIdCommand(int id) => this.Id = id;
}

public record GetByIdProductResponse(int Id, string Name, decimal Price);

public class GetByIdProductHandler : IHandler<GetProductByIdCommand, GetByIdProductResponse>
{
    private readonly IProductRepository _productRepository;
    public GetByIdProductHandler(IProductRepository productRepository) => _productRepository = productRepository;
    public async Task<GetByIdProductResponse> Handle(GetProductByIdCommand request)
    {
        var product = await _productRepository.GetProductByIdAsync(request.Id);

        if (product == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, "Product not found"));
        }

        return new GetByIdProductResponse(product.Id, product.Name.Value, product.Price.Value);
    }
}
