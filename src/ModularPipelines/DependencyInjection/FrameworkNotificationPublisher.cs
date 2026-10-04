using Mediator;

namespace ModularPipelines.DependencyInjection;

// Use a framework-owned DI type so consumer mediator lifetimes cannot replace it.
internal sealed class FrameworkNotificationPublisher : INotificationPublisher
{
    private readonly ForeachAwaitPublisher _publisher = new();

    public ValueTask Publish<TNotification>(NotificationHandlers<TNotification> handlers,
        TNotification notification, CancellationToken cancellationToken)
        where TNotification : INotification =>
        _publisher.Publish(handlers, notification, cancellationToken);
}
