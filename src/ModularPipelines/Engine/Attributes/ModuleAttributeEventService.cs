using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
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

    public IReadOnlyList<Attribute> GetAttributes(Type moduleType)
        => CreateAttributes(moduleType);

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
        return CreateHandlerCache(CreateAttributes(moduleType));
    }

    private static IReadOnlyList<Attribute> CreateAttributes(Type moduleType)
    {
        return GeneratedModuleEventMetadata.TryCreateAttributes(moduleType, out var generatedAttributes)
            ? generatedAttributes
            : GetAttributesWithReflection(moduleType);
    }

    private static AttributeHandlerCache CreateHandlerCache(IReadOnlyList<Attribute> attributes)
    {
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

    [UnconditionalSuppressMessage(
        "Trimming",
        "IL2026",
        Justification = "This fallback is used only for dynamic or plugin module types without generated metadata.")]
    private static IReadOnlyList<Attribute> GetAttributesWithReflection(Type moduleType)
        => DiscoverAttributesWithReflection(moduleType);

    [RequiresUnreferencedCode(
        "Reflection fallback requires module attribute constructors. Ensure ModularPipelines.SourceGenerator runs for trim-safe event invocation.")]
    private static IReadOnlyList<Attribute> DiscoverAttributesWithReflection(Type moduleType)
        => [.. moduleType.GetCustomAttributes(inherit: true).OfType<Attribute>()];

    private static IReadOnlyList<T> SortByPriority<T>(List<T> handlers)
    {
        if (handlers.Count <= 1)
        {
            return handlers;
        }

        // Use stable sort to preserve declaration order for handlers with same priority
        return [.. handlers.OrderBy(GetPriority)];
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
