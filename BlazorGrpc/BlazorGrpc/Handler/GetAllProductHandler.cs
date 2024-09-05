using BlazorAppData.Interrface;
using BlazorGrpc.Model;
using BlazorGrpcSimpleMediater;

namespace BlazorGrpc.Handler;

public class GetAllProductCommand : IRequest<List<GetAllProductResponse>>
{

}

public record GetAllProductResponse(int Id, string Name, decimal Price);

public class GetAllProductHandler : IHandler<GetAllProductCommand, List< GetAllProductResponse>>
{
    private readonly IProductRepository _productRepository;
    public GetAllProductHandler(IProductRepository productRepository) => _productRepository = productRepository;

    public async Task<List<GetAllProductResponse>> Handle(GetAllProductCommand request)
    {
        var products = await _productRepository.GetProductsAsync() ?? Enumerable.Empty<Product>();

        var response = new List<GetAllProductResponse>();

        response.AddRange(products.Select(x => new GetAllProductResponse(x.Id, x.Name.Value, x.Price.Value)));

        return response;
    }
}
