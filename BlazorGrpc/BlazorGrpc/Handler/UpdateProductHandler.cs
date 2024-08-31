using BlazorAppData.Interrface;
using BlazorGrpc.Model;
using BlazorGrpcSimpleMediater;
using Grpc.Core;

namespace BlazorGrpc.Handler;

public class UpdateProductCommand : IRequest<UpdateProductResponse>
{
    public int Id { get; }
    public string Name { get; }
    public decimal Price { get; }

    public UpdateProductCommand(int id ,string name, decimal price)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException($"'{nameof(name)}' cannot be null or empty.", nameof(name));
        }

        if (id <= 0)
            throw new ArgumentException($"'{nameof(id)}' cannot be null or empty.", nameof(id));

        Id = id;

        Name = name;

        Price = price;
    }
}

public record UpdateProductResponse(int Id, string Name, decimal Price);

public class UpdateProductHandler : IHandler<UpdateProductCommand, UpdateProductResponse>
{

    private readonly IProductRepository _productRepository;

    public UpdateProductHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }


    public async Task<UpdateProductResponse> Handle(UpdateProductCommand request)
    {
        var product = new Product
        {
            Id = request.Id,
            Name = request.Name,
            Price = (decimal)request.Price
        };

        await _productRepository.UpdateProductAsync(product);

        return new UpdateProductResponse(product.Id, product.Name, product.Price);
    }
}
