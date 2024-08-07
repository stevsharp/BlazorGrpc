namespace BlazorGrpcSimpleMediater;

public interface IMediator
{
    Task<TResponse> Send<TResponse>(IRequest<TResponse> request);

    Task PublishNotification<TEvent>(TEvent @event) where TEvent : INotification;
}