namespace BlazorGrpcSimpleMediater;

public interface IMediator
{
    Task<TResponse> Send<TResponse>(IRequest<TResponse> request);
}