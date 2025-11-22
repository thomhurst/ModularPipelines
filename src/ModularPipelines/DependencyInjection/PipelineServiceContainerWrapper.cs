using Microsoft.Extensions.DependencyInjection;

namespace ModularPipelines.DependencyInjection;

internal class PipelineServiceContainerWrapper : IPipelineServiceContainerWrapper
{
    public IServiceCollection ServiceCollection { get; }

    public PipelineServiceContainerWrapper(IServiceCollection serviceCollection)
    {
        ServiceCollection = serviceCollection;
    }
}
