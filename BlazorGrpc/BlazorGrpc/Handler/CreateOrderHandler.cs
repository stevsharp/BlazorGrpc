using BlazorAppData.Interrface;

using BlazorGrpc.Model;
using BlazorGrpcSimpleMediater;
using Grpc.Core;
namespace BlazorGrpc.Handler;

public class CreateProductCommand : IRequest<CreateProductResponse>
{
    public string Name { get;  }
    public decimal Price { get;  }

    public CreateProductCommand(string name, decimal price)
    {
        Name = name;
        Price = price;
    }
}

public record CreateProductResponse 
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
}


public class CreateOrderHandler : IHandler<CreateProductCommand, CreateProductResponse>
{
    private readonly IProductRepository _productRepository;
    public CreateOrderHandler(IProductRepository productRepository) => _productRepository = productRepository;

    public async Task<CreateProductResponse> Handle(CreateProductCommand request)
    {
        var product = new Product
        {
            Name = request.Name,
            Price = (decimal)request.Price
        };

        var isCreated = await _productRepository.CreateProdut(product);

        if (!isCreated)
            throw new RpcException(new Status(StatusCode.NotFound, "Product Could not be Created !!!!"));

        return new CreateProductResponse
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price
        };
    }
}
