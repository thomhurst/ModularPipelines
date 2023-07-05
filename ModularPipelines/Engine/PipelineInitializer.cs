using TomLonghurst.Microsoft.Extensions.DependencyInjection.ServiceInitialization.Extensions;

namespace ModularPipelines.Engine;

public class PipelineInitializer : IPipelineInitializer
{
    private readonly IServiceProvider _serviceProvider;

    public PipelineInitializer(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    public Task InitializeAsync()
    {
        return _serviceProvider.InitializeAsync();
    }
}
