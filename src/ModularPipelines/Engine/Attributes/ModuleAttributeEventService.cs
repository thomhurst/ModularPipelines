using System.Collections.Concurrent;
using System.Reflection;
using ModularPipelines.Attributes.Events;

namespace ModularPipelines.Engine.Attributes;

/// <summary>
/// Discovers and caches attribute event handlers on modules.
/// Handlers are returned sorted by priority (lower values first).
/// Handlers that implement <see cref="IEventHandlerPriority"/> are sorted by their <see cref="IEventHandlerPriority.Priority"/> value.
/// Handlers without priority default to 0.
/// </summary>
internal class ModuleAttributeEventService : IModuleAttributeEventService
{
    private readonly ConcurrentDictionary<Type, AttributeHandlerCache> _cache = new();

    public IReadOnlyList<IModuleRegistrationEventReceiver> GetRegistrationReceivers(Type moduleType)
        => GetCache(moduleType).RegistrationReceivers;

    public IReadOnlyList<IModuleReadyHandler> GetReadyHandlers(Type moduleType)
        => GetCache(moduleType).ReadyHandlers;

    public IReadOnlyList<IModuleStartHandler> GetStartHandlers(Type moduleType)
        => GetCache(moduleType).StartHandlers;

    public IReadOnlyList<IModuleEndHandler> GetEndHandlers(Type moduleType)
        => GetCache(moduleType).EndHandlers;

    public IReadOnlyList<IModuleFailureHandler> GetFailureHandlers(Type moduleType)
        => GetCache(moduleType).FailureHandlers;

    public IReadOnlyList<IModuleSkippedHandler> GetSkippedHandlers(Type moduleType)
        => GetCache(moduleType).SkippedHandlers;

    private AttributeHandlerCache GetCache(Type moduleType)
        => _cache.GetOrAdd(moduleType, DiscoverHandlers);

    private static AttributeHandlerCache DiscoverHandlers(Type moduleType)
    {
        // Get attributes in declaration order
        var attributes = moduleType.GetCustomAttributes(inherit: true);

        var registrationReceivers = new List<IModuleRegistrationEventReceiver>();
        var readyHandlers = new List<IModuleReadyHandler>();
        var startHandlers = new List<IModuleStartHandler>();
        var endHandlers = new List<IModuleEndHandler>();
        var failureHandlers = new List<IModuleFailureHandler>();
        var skippedHandlers = new List<IModuleSkippedHandler>();

        foreach (var attribute in attributes)
        {
            if (attribute is IModuleRegistrationEventReceiver registration)
            {
                registrationReceivers.Add(registration);
            }

            if (attribute is IModuleReadyHandler ready)
            {
                readyHandlers.Add(ready);
            }

            if (attribute is IModuleStartHandler start)
            {
                startHandlers.Add(start);
            }

            if (attribute is IModuleEndHandler end)
            {
                endHandlers.Add(end);
            }

            if (attribute is IModuleFailureHandler failure)
            {
                failureHandlers.Add(failure);
            }

            if (attribute is IModuleSkippedHandler skipped)
            {
                skippedHandlers.Add(skipped);
            }
        }

        // Sort all handlers by priority (lower values first)
        // Handlers without IEventHandlerPriority default to 0
        return new AttributeHandlerCache(
            SortByPriority(registrationReceivers),
            SortByPriority(readyHandlers),
            SortByPriority(startHandlers),
            SortByPriority(endHandlers),
            SortByPriority(failureHandlers),
            SortByPriority(skippedHandlers));
    }

    private static IReadOnlyList<T> SortByPriority<T>(List<T> handlers)
    {
        if (handlers.Count <= 1)
        {
            return handlers;
        }

        // Use stable sort to preserve declaration order for handlers with same priority
        return handlers
            .OrderBy(r => GetPriority(r))
            .ToList();
    }

    private static int GetPriority<T>(T handler)
        => handler is IEventHandlerPriority prioritized ? prioritized.Priority : 0;

    private sealed record AttributeHandlerCache(
        IReadOnlyList<IModuleRegistrationEventReceiver> RegistrationReceivers,
        IReadOnlyList<IModuleReadyHandler> ReadyHandlers,
        IReadOnlyList<IModuleStartHandler> StartHandlers,
        IReadOnlyList<IModuleEndHandler> EndHandlers,
        IReadOnlyList<IModuleFailureHandler> FailureHandlers,
        IReadOnlyList<IModuleSkippedHandler> SkippedHandlers);
}
