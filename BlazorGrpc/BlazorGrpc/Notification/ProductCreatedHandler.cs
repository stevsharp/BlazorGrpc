using BlazorGrpcSimpleMediater;

namespace BlazorGrpc.Notification;

public class ProducCreatedNotificationHandler : INotificationHandler<ProductCreatedNotification>
{
    public Task Publish(ProductCreatedNotification notification)
    {
        Console.WriteLine($"Event received: {notification.Message}");

        return Task.CompletedTask;
    }
}
