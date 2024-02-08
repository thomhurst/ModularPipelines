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
            .Where(t => t.GetGenericTypeDefinition().IsAssignableTo(typeof(IConfigureOptions<>)) || t.GetGenericTypeDefinition().IsAssignableTo(typeof(IPostConfigureOptions<>)))
            .Select(s => s.GetGenericArguments()[0])
            .ToList();

        foreach (var option in types.Select(t => _serviceProvider.GetService(typeof(IOptions<>).MakeGenericType([t]))))
        {
            yield return option!.GetType().GetProperty("Value", BindingFlags.Public | BindingFlags.Instance)!.GetValue(option);
        }
    }
}