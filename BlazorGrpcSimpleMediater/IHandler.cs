namespace BlazorGrpcSimpleMediater;

public interface IHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    Task<TResponse> Handle(TRequest request);
}
