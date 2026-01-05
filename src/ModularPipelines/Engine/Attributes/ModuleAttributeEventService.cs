using System.Collections.Concurrent;
using System.Reflection;
using ModularPipelines.Attributes.Events;

namespace ModularPipelines.Engine.Attributes;

/// <summary>
/// Discovers and caches attribute event receivers on modules.
/// Receivers are returned sorted by priority (lower values first).
/// Receivers that implement <see cref="IEventHandlerPriority"/> are sorted by their <see cref="IEventHandlerPriority.Priority"/> value.
/// Receivers without priority default to 0.
/// </summary>
internal class ModuleAttributeEventService : IModuleAttributeEventService
{
    private readonly ConcurrentDictionary<Type, AttributeReceiverCache> _cache = new();

    public IReadOnlyList<IModuleRegistrationEventReceiver> GetRegistrationReceivers(Type moduleType)
        => GetCache(moduleType).RegistrationReceivers;

    public IReadOnlyList<IModuleReadyEventReceiver> GetReadyReceivers(Type moduleType)
        => GetCache(moduleType).ReadyReceivers;

    public IReadOnlyList<IModuleStartEventReceiver> GetStartReceivers(Type moduleType)
        => GetCache(moduleType).StartReceivers;

    public IReadOnlyList<IModuleEndEventReceiver> GetEndReceivers(Type moduleType)
        => GetCache(moduleType).EndReceivers;

    public IReadOnlyList<IModuleFailureEventReceiver> GetFailureReceivers(Type moduleType)
        => GetCache(moduleType).FailureReceivers;

    public IReadOnlyList<IModuleSkippedEventReceiver> GetSkippedReceivers(Type moduleType)
        => GetCache(moduleType).SkippedReceivers;

    private AttributeReceiverCache GetCache(Type moduleType)
        => _cache.GetOrAdd(moduleType, DiscoverReceivers);

    private static AttributeReceiverCache DiscoverReceivers(Type moduleType)
    {
        // Get attributes in declaration order
        var attributes = moduleType.GetCustomAttributes(inherit: true);

        var registrationReceivers = new List<IModuleRegistrationEventReceiver>();
        var readyReceivers = new List<IModuleReadyEventReceiver>();
        var startReceivers = new List<IModuleStartEventReceiver>();
        var endReceivers = new List<IModuleEndEventReceiver>();
        var failureReceivers = new List<IModuleFailureEventReceiver>();
        var skippedReceivers = new List<IModuleSkippedEventReceiver>();

        foreach (var attribute in attributes)
        {
            if (attribute is IModuleRegistrationEventReceiver registration)
            {
                registrationReceivers.Add(registration);
            }

            if (attribute is IModuleReadyEventReceiver ready)
            {
                readyReceivers.Add(ready);
            }

            if (attribute is IModuleStartEventReceiver start)
            {
                startReceivers.Add(start);
            }

            if (attribute is IModuleEndEventReceiver end)
            {
                endReceivers.Add(end);
            }

            if (attribute is IModuleFailureEventReceiver failure)
            {
                failureReceivers.Add(failure);
            }

            if (attribute is IModuleSkippedEventReceiver skipped)
            {
                skippedReceivers.Add(skipped);
            }
        }

        // Sort all receivers by priority (lower values first)
        // Receivers without IEventHandlerPriority default to 0
        return new AttributeReceiverCache(
            SortByPriority(registrationReceivers),
            SortByPriority(readyReceivers),
            SortByPriority(startReceivers),
            SortByPriority(endReceivers),
            SortByPriority(failureReceivers),
            SortByPriority(skippedReceivers));
    }

    private static IReadOnlyList<T> SortByPriority<T>(List<T> receivers)
    {
        if (receivers.Count <= 1)
        {
            return receivers;
        }

        // Use stable sort to preserve declaration order for receivers with same priority
        return receivers
            .OrderBy(r => GetPriority(r))
            .ToList();
    }

    private static int GetPriority<T>(T receiver)
        => receiver is IEventHandlerPriority prioritized ? prioritized.Priority : 0;

    private sealed record AttributeReceiverCache(
        IReadOnlyList<IModuleRegistrationEventReceiver> RegistrationReceivers,
        IReadOnlyList<IModuleReadyEventReceiver> ReadyReceivers,
        IReadOnlyList<IModuleStartEventReceiver> StartReceivers,
        IReadOnlyList<IModuleEndEventReceiver> EndReceivers,
        IReadOnlyList<IModuleFailureEventReceiver> FailureReceivers,
        IReadOnlyList<IModuleSkippedEventReceiver> SkippedReceivers);
}
