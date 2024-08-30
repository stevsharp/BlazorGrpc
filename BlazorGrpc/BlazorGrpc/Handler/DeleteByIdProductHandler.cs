using BlazorAppData.Interrface;
using BlazorGrpc.Model;

using BlazorGrpcSimpleMediater;

using Grpc.Core;

namespace BlazorGrpc.Handler;

public class DeleteProductByIdCommand : IRequest<bool>
{
    public int Id { get; }

    public DeleteProductByIdCommand(int id) => this.Id = id;
}


public class DeleteByIdProductHandler : IHandler<DeleteProductByIdCommand, bool>
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork<int> _unitOfWork;
    public DeleteByIdProductHandler(IProductRepository productRepository, IUnitOfWork<int> unitOfWork)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task<bool> Handle(DeleteProductByIdCommand request)
    {
        var product = new Product
        {
            Id = request.Id
        };

        await _productRepository.DeleteProduct(product);

        var isDeleted = await _unitOfWork.Commit(CancellationToken.None) > 0;

        if (!isDeleted)
            throw new RpcException(new Status(StatusCode.NotFound, "Product Could not be Deleted !!!!"));

        return isDeleted;
    }
}

