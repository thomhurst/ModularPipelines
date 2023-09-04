using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.DependencyInjection;
using TomLonghurst.Microsoft.Extensions.DependencyInjection.ServiceInitialization.Extensions;

namespace ModularPipelines.Engine;

internal class ServiceProviderInitializer : IServiceProviderInitializer
{
    private readonly IServiceProvider _serviceProvider;
    private readonly SecretsLogFilter _secretsLogFilter;
    private readonly ISecretProvider _secretProvider;

    public ServiceProviderInitializer(IServiceProvider serviceProvider,
        SecretsLogFilter secretsLogFilter,
        ISecretProvider secretProvider)
    {
        _serviceProvider = serviceProvider;
        _secretsLogFilter = secretsLogFilter;
        _secretProvider = secretProvider;
    }

    public async Task InitializeAsync()
    {
        await _serviceProvider.InitializeAsync();
        _secretsLogFilter.SecretProvider = _secretProvider;
    }
}
