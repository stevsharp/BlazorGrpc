using BlazorGrpcSimpleMediater;

namespace BlazorGrpc.Notification
{
    public class ProductCreatedNotification : INotification
    {
        public string Message { get; }

        public ProductCreatedNotification(string message) => Message = message;
    }
}
