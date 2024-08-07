
namespace BlazorGrpcSimpleMediater;

public class Mediator : IMediator
{
    private readonly IServiceProvider _serviceProvider;

    public Mediator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request)
    {
        var handlerType = typeof(IHandler<,>).MakeGenericType(request.GetType(), typeof(TResponse));
        dynamic? handler = _serviceProvider.GetService(handlerType);

        if (handler is not null)
        {
            return await handler.Handle((dynamic)request);
        }

        throw new InvalidOperationException($"Not Valid Handler,  type :  {request.GetType()}");

    }
}