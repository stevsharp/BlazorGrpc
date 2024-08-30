

using BlazorAppData.Interrface;

using BlazorGrpc.Model;

using BlazorGrpcSimpleMediater;

using LazyCache;

namespace BlazorAppData.UnitOfWork;

public class UnitOfWork<TId> : IUnitOfWork<TId>
{
    private readonly ProductContext _productContext;

    private readonly IMediator _mediator;

    private readonly IAppCache _cache;

    private bool disposed;

    public UnitOfWork(ProductContext productContext , 
        IMediator mediator,
        IAppCache cache)
    {
        _productContext = productContext;

        _mediator = mediator;

        _cache = cache;
    }
    public async Task<int> Commit(CancellationToken cancellationToken)
    {
        return await _productContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<int> CommitAndRemoveCache(CancellationToken cancellationToken, params string[] cacheKeys)
    {
        var result = await _productContext.SaveChangesAsync(cancellationToken);

        for (int i = 0; i < cacheKeys.Length; i++)
        {
            string? cacheKey = cacheKeys[i];

            _cache.Remove(cacheKey);
        }

        return result;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
            {
                _productContext.Dispose();
            }
        }

        disposed = true;
    }
    public Task Rollback()
    {
        _productContext.ChangeTracker.Entries().ToList().ForEach(x => x.Reload());

        return Task.CompletedTask;
    }
}
