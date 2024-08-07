

namespace BlazorGrpcSimpleMediater;

public interface INotificationHandler<in TNotification> where TNotification  : INotification
{
    Task Publish(TNotification notification); 
}
