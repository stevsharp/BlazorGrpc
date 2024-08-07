using BlazorAppData.Interrface;
using BlazorGrpc.Model;

using BlazorGrpcSimpleMediater;

namespace BlazorGrpc.Handler;

public class DeleteProductByIdCommand : IRequest<bool>
{
    public int Id { get; }

    public DeleteProductByIdCommand(int id) => this.Id = id;
}


public class DeleteByIdProductHandler : IHandler<DeleteProductByIdCommand, bool>
{
    private readonly IProductRepository _productRepository;
    public DeleteByIdProductHandler(IProductRepository productRepository) => _productRepository = productRepository;
    public async Task<bool> Handle(DeleteProductByIdCommand request)
    {
        var product = new Product
        {
            Id = request.Id
        };

        var isDeleted = await _productRepository.DeleteProduct(product);


        return isDeleted;
    }
}

