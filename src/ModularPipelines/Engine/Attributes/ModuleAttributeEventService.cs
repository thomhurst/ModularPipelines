using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;
using ModularPipelines.Events;
using ModularPipelines.Exceptions;

using ModularPipelines.Generated;

namespace ModularPipelines.Engine.Attributes;

/// <summary>
/// Discovers and caches attribute event handlers on modules.
/// Handlers are returned sorted by priority (lower values first).
/// The default <see cref="IEventHandler.Priority"/> is 0.
/// </summary>
internal class ModuleAttributeEventService : IModuleAttributeEventService
{
    private readonly ConcurrentDictionary<Type, Lazy<AttributeHandlerCache>> _cache = new();
    private readonly ConcurrentDictionary<Type, Lazy<PlanningAttributeCache>> _planningCache = new();

    public IReadOnlyList<Attribute> GetAttributes(Type moduleType)
        => GetCache(moduleType).Attributes;

    public IReadOnlyList<IModuleRegistrationHandler> GetRegistrationHandlers(Type moduleType)
        => GetCache(moduleType).RegistrationHandlers;

    public IReadOnlyList<IModuleRegistrationHandler> GetPlanningRegistrationHandlers(Type moduleType)
        => GetPlanningCache(moduleType).RegistrationHandlers;

    public IReadOnlyList<Attribute> GetPlanningAttributes(Type moduleType)
        => GetPlanningCache(moduleType).Attributes;

    private PlanningAttributeCache GetPlanningCache(Type moduleType)
        => _planningCache.GetOrAdd(
            moduleType,
            static type => new Lazy<PlanningAttributeCache>(
                () => DiscoverPlanningAttributes(type),
                LazyThreadSafetyMode.ExecutionAndPublication)).Value;

    private static PlanningAttributeCache DiscoverPlanningAttributes(Type moduleType)
    {
        var handlerData = CustomAttributeMetadata.GetApplicable(
            moduleType,
            static type => typeof(IModuleRegistrationHandler).IsAssignableFrom(type));
        if (handlerData.Count == 0)
        {
            return new PlanningAttributeCache([], []);
        }

        var attributeData = CustomAttributeMetadata.GetApplicable(moduleType, static _ => true);
        var attributes = attributeData.Select(CreatePlanningAttribute).ToArray();
        var handlers = attributes.OfType<IModuleRegistrationHandler>().ToList();
        var unsafeHandlerTypes = handlers
            .Where(static handler => !handler.IsPlanningSafe)
            .Select(static handler => handler.GetType())
            .Distinct()
            .ToArray();
        if (unsafeHandlerTypes.Length > 0)
        {
            throw new PipelineException(
                $"Cannot export a resolved dependency graph because {moduleType.FullName} has "
                + "registration handlers that are not planning-safe: "
                + string.Join(", ", unsafeHandlerTypes.Select(static type => type.FullName))
                + $". Set {nameof(IModuleRegistrationHandler.IsPlanningSafe)} to true only when "
                + "the handler is deterministic, idempotent, and free of external side effects.");
        }

        return new PlanningAttributeCache(attributes, SortByPriority(handlers));
    }

    [UnconditionalSuppressMessage(
        "Trimming",
        "IL2072",
        Justification = "The exact attribute type is preserved by the custom-attribute metadata being inspected.")]
    private static Attribute CreatePlanningAttribute(CustomAttributeData data)
    {
        if (typeof(IModuleRegistrationHandler).IsAssignableFrom(data.AttributeType)
            || data.AttributeType.Assembly == typeof(ModuleAttributeEventService).Assembly)
        {
            return CustomAttributeMetadata.Create<Attribute>(data);
        }

        if (!IsCompilerMetadataAttribute(data.AttributeType)
            && (data.ConstructorArguments.Count > 0
            || data.NamedArguments.Count > 0
            || HasInstanceState(data.AttributeType)))
        {
            throw new PipelineException(
                $"Cannot export a resolved dependency graph because {data.AttributeType.FullName} is a stateful "
                + "companion to a planning-safe registration handler. Planning cannot construct arbitrary "
                + "companion attributes. Use a stateless marker attribute or move the required state onto the "
                + $"{nameof(IModuleRegistrationHandler)} attribute.");
        }

        return (Attribute) RuntimeHelpers.GetUninitializedObject(data.AttributeType);
    }

    private static bool IsCompilerMetadataAttribute(Type attributeType)
        => attributeType.Namespace == typeof(CompilerGeneratedAttribute).Namespace;

    [UnconditionalSuppressMessage(
        "Trimming",
        "IL2070",
        Justification = "Planning inspects module attribute types that are already preserved by custom-attribute metadata.")]
    [UnconditionalSuppressMessage(
        "Trimming",
        "IL2075",
        Justification = "Planning inspects module attribute types that are already preserved by custom-attribute metadata.")]
    private static bool HasInstanceState(Type attributeType)
    {
        for (var current = attributeType; current != typeof(Attribute); current = current.BaseType)
        {
            if (current is null)
            {
                return true;
            }

            if (current.GetFields(
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly).Length > 0)
            {
                return true;
            }
        }

        return false;
    }

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
        => _cache.GetOrAdd(
            moduleType,
            static type => new Lazy<AttributeHandlerCache>(
                () => DiscoverHandlers(type),
                LazyThreadSafetyMode.ExecutionAndPublication)).Value;

    private static AttributeHandlerCache DiscoverHandlers(Type moduleType)
    {
        var attributes = CreateAttributes(moduleType);
        return CreateHandlerCache(attributes);
    }

    private static IReadOnlyList<Attribute> CreateAttributes(Type moduleType)
    {
        return GeneratedModuleEventMetadata.TryCreateAttributes(moduleType, out var generatedAttributes)
            ? generatedAttributes
            : GetAttributesWithReflection(moduleType);
    }

    private static AttributeHandlerCache CreateHandlerCache(IReadOnlyList<Attribute> attributes)
    {
        var registrationHandlers = new List<IModuleRegistrationHandler>();
        var readyHandlers = new List<IModuleReadyHandler>();
        var startHandlers = new List<IModuleStartHandler>();
        var endHandlers = new List<IModuleEndHandler>();
        var failureHandlers = new List<IModuleFailureHandler>();
        var skippedHandlers = new List<IModuleSkippedHandler>();

        foreach (var attribute in attributes)
        {
            if (attribute is IModuleRegistrationHandler registrationHandler)
            {
                registrationHandlers.Add(registrationHandler);
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

        return new AttributeHandlerCache(
            attributes,
            SortByPriority(registrationHandlers),
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
        where T : IEventHandler
    {
        if (handlers.Count <= 1)
        {
            return handlers;
        }

        // Use stable sort to preserve declaration order for handlers with same priority
        return [.. handlers.OrderBy(static handler => handler.Priority)];
    }

    private sealed record AttributeHandlerCache(
        IReadOnlyList<Attribute> Attributes,
        IReadOnlyList<IModuleRegistrationHandler> RegistrationHandlers,
        IReadOnlyList<IModuleReadyHandler> ReadyHandlers,
        IReadOnlyList<IModuleStartHandler> StartHandlers,
        IReadOnlyList<IModuleEndHandler> EndHandlers,
        IReadOnlyList<IModuleFailureHandler> FailureHandlers,
        IReadOnlyList<IModuleSkippedHandler> SkippedHandlers);

    private sealed record PlanningAttributeCache(
        IReadOnlyList<Attribute> Attributes,
        IReadOnlyList<IModuleRegistrationHandler> RegistrationHandlers);
}
