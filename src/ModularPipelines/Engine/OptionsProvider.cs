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

    private readonly IPipelineServiceContainerWrapper _pipelineServiceContainerWrapper;
    private readonly IServiceProvider _serviceProvider;

    public OptionsProvider(IPipelineServiceContainerWrapper pipelineServiceContainerWrapper, IServiceProvider serviceProvider)
    {
        _pipelineServiceContainerWrapper = pipelineServiceContainerWrapper;
        _serviceProvider = serviceProvider;
    }

    public IEnumerable<object?> GetOptions()
    {
        var types = _pipelineServiceContainerWrapper.ServiceCollection
            .Select(sd => sd.ServiceType)
            .Where(t => t.IsGenericType)
            .Where(x => x.IsConstructedGenericType)
            .Where(t =>
            {
                var genericTypeDefinition = t.GetGenericTypeDefinition();

                return genericTypeDefinition.IsAssignableTo(typeof(IConfigureOptions<>))
                       || genericTypeDefinition.IsAssignableTo(typeof(IPostConfigureOptions<>))
                       || genericTypeDefinition.IsAssignableTo(typeof(IOptions<>))
                       || genericTypeDefinition.IsAssignableTo(typeof(IOptionsMonitor<>))
                       || genericTypeDefinition.IsAssignableTo(typeof(IOptionsSnapshot<>))
                       || genericTypeDefinition.IsAssignableTo(typeof(IValidateOptions<>))
                       || genericTypeDefinition.IsAssignableTo(typeof(IConfigureNamedOptions<>));
            })
            .Select(s => s.GetGenericArguments()[0])
            .Distinct()
            .ToList();

        foreach (var option in types.Select(t => _serviceProvider.GetService(typeof(IOptions<>).MakeGenericType(t))))
        {
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
}