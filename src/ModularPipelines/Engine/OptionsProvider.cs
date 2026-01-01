using System.Collections.Concurrent;
using System.Linq.Expressions;
using Microsoft.Extensions.Options;
using ModularPipelines.DependencyInjection;

namespace ModularPipelines.Engine;

internal class OptionsProvider : IOptionsProvider
{
    /// <summary>
    /// Cache of compiled property accessors for IOptions&lt;T&gt;.Value.
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
    /// </summary>
    private static readonly HashSet<Type> OptionsTypeDefinitions = new()
    {
        typeof(IConfigureOptions<>),
        typeof(IPostConfigureOptions<>),
        typeof(IOptions<>),
        typeof(IOptionsMonitor<>),
        typeof(IOptionsSnapshot<>),
        typeof(IValidateOptions<>),
        typeof(IConfigureNamedOptions<>),
    };

    private readonly IPipelineServiceContainerWrapper _pipelineServiceContainerWrapper;
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Cached list of option types. Populated lazily on first access since
    /// the service collection does not change after startup.
    /// </summary>
    private List<Type>? _cachedOptionTypes;

    public OptionsProvider(IPipelineServiceContainerWrapper pipelineServiceContainerWrapper, IServiceProvider serviceProvider)
    {
        _pipelineServiceContainerWrapper = pipelineServiceContainerWrapper;
        _serviceProvider = serviceProvider;
    }

    public IEnumerable<object?> GetOptions()
    {
        // Cache the types list since service collection doesn't change after startup
        _cachedOptionTypes ??= _pipelineServiceContainerWrapper.ServiceCollection
            .Select(sd => sd.ServiceType)
            .Where(t => t.IsGenericType && t.IsConstructedGenericType && IsOptionsType(t.GetGenericTypeDefinition()))
            .Select(s => s.GetGenericArguments()[0])
            .Distinct()
            .ToList();

        foreach (var t in _cachedOptionTypes)
        {
            var optionsType = IOptionsTypeCache.GetOrAdd(t, static innerType => typeof(IOptions<>).MakeGenericType(innerType));
            var option = _serviceProvider.GetService(optionsType);

            if (option is null)
            {
                continue;
            }

            // Use cached compiled delegate instead of reflection
            var getter = ValueGetterCache.GetOrAdd(option.GetType(), CreateValueGetter);
            yield return getter(option);
        }
    }

    /// <summary>
    /// Creates a compiled delegate to access the Value property of an IOptions&lt;T&gt; instance.
    /// </summary>
    private static Func<object, object?> CreateValueGetter(Type optionsType)
    {
        var valueProperty = optionsType.GetProperty("Value")
            ?? throw new InvalidOperationException($"Property 'Value' not found on type '{optionsType.Name}'.");

        // Build: (object obj) => (object?)((OptionsType)obj).Value
        var param = Expression.Parameter(typeof(object), "obj");
        var cast = Expression.Convert(param, optionsType);
        var propertyAccess = Expression.Property(cast, valueProperty);
        var convertToObject = Expression.Convert(propertyAccess, typeof(object));

        return Expression.Lambda<Func<object, object?>>(convertToObject, param).Compile();
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