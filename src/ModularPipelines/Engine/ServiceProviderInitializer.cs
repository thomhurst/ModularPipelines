using ModularPipelines.DependencyInjection;
using TomLonghurst.Microsoft.Extensions.DependencyInjection.ServiceInitialization.Extensions;

namespace ModularPipelines.Engine;

internal class ServiceProviderInitializer(IServiceProvider serviceProvider) : IServiceProviderInitializer
{
    public async Task InitializeAsync()
    {
        await serviceProvider.InitializeAsync();
    }
}
