

namespace BlazorAppData.Interrface;

public interface IUnitOfWork<TId> : IDisposable
{
    Task<int> Commit(CancellationToken cancellationToken);

    Task<int> CommitAndRemoveCache(CancellationToken cancellationToken, params string[] cacheKeys);

    Task Rollback();
}
