using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.ExceptionServices;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines.DependencyInjection;
using ModularPipelines.Distributed;
using ModularPipelines.Options;
using ModularPipelines.Secrets;

namespace ModularPipelines.Engine;

/// <summary>
/// Provides access to all configured options in the service container.
/// </summary>
/// <remarks>
/// <para>
/// <b>Thread Safety:</b> This class is thread-safe. All public methods can be called
/// concurrently from multiple threads without external synchronization.
/// </para>
/// <para>
/// The options type cache uses <see cref="Lazy{T}"/> for thread-safe initialization,
/// and all static caches use <see cref="ConcurrentDictionary{TKey,TValue}"/>.
/// </para>
/// </remarks>
/// <threadsafety static="true" instance="true"/>
internal class OptionsProvider : IOptionsProvider
{
    /// <summary>
    /// Cache of property accessors for IOptions&lt;T&gt;.Value.
    /// Avoids repeated reflection for property access.
    /// </summary>
    private static readonly ConcurrentDictionary<Type, Func<object, object?>> ValueGetterCache = new();

    /// <summary>
    /// Cache of constructed IOptions&lt;T&gt; types to avoid repeated MakeGenericType calls.
    /// </summary>
    private static readonly ConcurrentDictionary<Type, Type> IOptionsTypeCache = new();

    /// <summary>
    /// Set of generic type definitions that indicate an options-related type.
    /// Using a HashSet for O(1) lookup instead of multiple IsAssignableTo calls.
    /// This is read-only after static initialization so no synchronization needed.
    /// </summary>
    private static readonly HashSet<Type> OptionsTypeDefinitions =
    [
        typeof(IConfigureOptions<>),
        typeof(IPostConfigureOptions<>),
        typeof(IOptions<>),
        typeof(IOptionsMonitor<>),
        typeof(IOptionsSnapshot<>),
        typeof(IValidateOptions<>),
        typeof(IConfigureNamedOptions<>),
    ];

    private readonly IPipelineServiceContainerWrapper _pipelineServiceContainerWrapper;
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Thread-safe lazy-initialized cache of option types. Populated on first access since
    /// the service collection does not change after startup.
    /// </summary>
    private readonly Lazy<IReadOnlyList<Type>> _cachedOptionTypes;

    public OptionsProvider(IPipelineServiceContainerWrapper pipelineServiceContainerWrapper, IServiceProvider serviceProvider)
    {
        _pipelineServiceContainerWrapper = pipelineServiceContainerWrapper;
        _serviceProvider = serviceProvider;

        // Use Lazy<T> for thread-safe initialization
        _cachedOptionTypes = new Lazy<IReadOnlyList<Type>>(
            () => [.. _pipelineServiceContainerWrapper.ServiceCollection
                .Select(sd => sd.ServiceType)
                .Where(t => t.IsGenericType && t.IsConstructedGenericType && IsOptionsType(t.GetGenericTypeDefinition()))
                .Select(s => s.GetGenericArguments()[0])
                .Distinct()],
            LazyThreadSafetyMode.ExecutionAndPublication);
    }

    /// <summary>
    /// Gets all configured options from the service container.
    /// </summary>
    /// <returns>An enumerable of option values.</returns>
    /// <remarks>
    /// This method is thread-safe. The options type list is cached on first access.
    /// </remarks>
    [DynamicDependency(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor, typeof(PipelineOptions))]
    [DynamicDependency(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor, typeof(SchedulerOptions))]
    [DynamicDependency(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor, typeof(ConcurrencyOptions))]
    [DynamicDependency(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor, typeof(HttpResilienceOptions))]
    [DynamicDependency(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor, typeof(SecretMaskingOptions))]
    [DynamicDependency(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor, typeof(DistributedOptions))]
    [DynamicDependency(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor, typeof(ArtifactOptions))]
    [DynamicDependency(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor, typeof(LoggerFilterOptions))]
    public IEnumerable<object?> GetOptions()
    {
        foreach (var t in _cachedOptionTypes.Value)
        {
            var optionsType = IOptionsTypeCache.GetOrAdd(t, CreateOptionsType);
            var option = _serviceProvider.GetService(optionsType);

            if (option is null)
            {
                continue;
            }

            // Cache the interface property lookup used for each options type
            var getter = ValueGetterCache.GetOrAdd(optionsType, CreateValueGetter);
            yield return getter(option);
        }
    }

    /// <summary>
    /// Creates the closed IOptions type for a runtime-discovered options type.
    /// </summary>
    [UnconditionalSuppressMessage(
        "AOT",
        "IL3050",
        Justification = "Options registered through the static DI path are rooted; runtime-discovered option types are unsupported in Native AOT.")]
    [UnconditionalSuppressMessage(
        "Trimming",
        "IL2071",
        Justification = "Options registered through the static DI path are rooted; runtime-discovered option types are unsupported when trimming.")]
    private static Type CreateOptionsType(Type optionsType)
    {
        return typeof(IOptions<>).MakeGenericType(optionsType);
    }

    /// <summary>
    /// Creates a delegate to access the Value property of an IOptions&lt;T&gt; instance.
    /// </summary>
    [DynamicDependency(DynamicallyAccessedMemberTypes.PublicProperties, typeof(IOptions<>))]
    [UnconditionalSuppressMessage(
        "Trimming",
        "IL2070",
        Justification = "Options registered through the static DI path are rooted; runtime-discovered option types are unsupported when trimming.")]
    private static Func<object, object?> CreateValueGetter(Type optionsType)
    {
        var valueProperty = optionsType.GetProperty("Value")
            ?? throw new InvalidOperationException($"Property 'Value' not found on type '{optionsType.Name}'.");

        return instance =>
        {
            try
            {
                return valueProperty.GetValue(instance);
            }
            catch (TargetInvocationException exception) when (exception.InnerException is not null)
            {
                ExceptionDispatchInfo.Capture(exception.InnerException).Throw();
                throw;
            }
        };
    }

    /// <summary>
    /// Checks if the given generic type definition is an options-related type.
    /// Uses HashSet for O(1) lookup instead of multiple IsAssignableTo calls.
    /// </summary>
    private static bool IsOptionsType(Type genericTypeDefinition)
    {
        return OptionsTypeDefinitions.Contains(genericTypeDefinition);
    }
}
