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

    private readonly IUnitOfWork<int> _unitOfWork;

    public UpdateProductHandler(IProductRepository productRepository, IUnitOfWork<int> unitOfWork)
    {
        _productRepository = productRepository;

        _unitOfWork = unitOfWork;

    }


    public async Task<UpdateProductResponse> Handle(UpdateProductCommand request)
    {
        var product = new Product
        {
            Id = request.Id,
            Name = request.Name,
            Price = (decimal)request.Price
        };

         await _productRepository.UpdateProduct(product);

        var isUpdated = await _unitOfWork.Commit(CancellationToken.None) > 0;

        if (!isUpdated)
            throw new RpcException(new Status(StatusCode.NotFound, "Product Could not be Updated !!!"));

        return new UpdateProductResponse(product.Id, product.Name, product.Price);
    }
}
