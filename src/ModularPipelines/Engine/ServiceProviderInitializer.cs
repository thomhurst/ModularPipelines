using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.DependencyInjection;
using TomLonghurst.Microsoft.Extensions.DependencyInjection.ServiceInitialization.Extensions;

namespace ModularPipelines.Engine;

internal class ServiceProviderInitializer : IServiceProviderInitializer
{
    private readonly IServiceProvider _serviceProvider;

    public ServiceProviderInitializer(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task InitializeAsync()
    {
        await _serviceProvider.InitializeAsync();
        SecretsLogFilter.SecretProvider = _serviceProvider.GetRequiredService<ISecretProvider>();
    }
}
