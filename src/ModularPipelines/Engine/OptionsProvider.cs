using System.Reflection;
using Microsoft.Extensions.Options;
using ModularPipelines.DependencyInjection;

namespace ModularPipelines.Engine;

internal class OptionsProvider : IOptionsProvider
{
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
            yield return option!.GetType().GetProperty("Value", BindingFlags.Public | BindingFlags.Instance)!.GetValue(option);
        }
    }
}