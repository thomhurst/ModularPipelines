using TomLonghurst.Microsoft.Extensions.DependencyInjection.ServiceInitialization.Extensions;

namespace ModularPipelines.Engine;

internal class ServiceProviderInitializer : IServiceProviderInitializer
{
    private readonly IServiceProvider _serviceProvider;

    public ServiceProviderInitializer(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Task InitializeAsync()
    {
        return _serviceProvider.InitializeAsync();
    }
}
