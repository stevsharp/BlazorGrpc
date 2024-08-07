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
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException($"'{nameof(name)}' cannot be null or empty.", nameof(name));
        }

        Name = name;

        Price = price;
    }
}

public record CreateProductResponse(int Id, string Name, decimal Price);

public class CreateProductHandler : IHandler<CreateProductCommand, CreateProductResponse>
{
    private readonly IProductRepository _productRepository;
    public CreateProductHandler(IProductRepository productRepository) => _productRepository = productRepository;

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

        return new CreateProductResponse(product.Id, product.Name, product.Price);
    }
}
