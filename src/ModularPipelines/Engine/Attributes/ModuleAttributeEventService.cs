using System.Collections.Concurrent;
using System.Reflection;
using ModularPipelines.Attributes.Events;

namespace ModularPipelines.Engine.Attributes;

/// <summary>
/// Discovers and caches attribute event receivers on modules.
/// Receivers are returned in declaration order.
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

        return new AttributeReceiverCache(
            registrationReceivers,
            readyReceivers,
            startReceivers,
            endReceivers,
            failureReceivers,
            skippedReceivers);
    }

    private sealed record AttributeReceiverCache(
        IReadOnlyList<IModuleRegistrationEventReceiver> RegistrationReceivers,
        IReadOnlyList<IModuleReadyEventReceiver> ReadyReceivers,
        IReadOnlyList<IModuleStartEventReceiver> StartReceivers,
        IReadOnlyList<IModuleEndEventReceiver> EndReceivers,
        IReadOnlyList<IModuleFailureEventReceiver> FailureReceivers,
        IReadOnlyList<IModuleSkippedEventReceiver> SkippedReceivers);
}
