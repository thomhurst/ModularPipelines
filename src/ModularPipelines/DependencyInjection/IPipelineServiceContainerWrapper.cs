using Microsoft.Extensions.DependencyInjection;

namespace ModularPipelines.DependencyInjection;

internal interface IPipelineServiceContainerWrapper
{
    IServiceCollection ServiceCollection { get; }
}
