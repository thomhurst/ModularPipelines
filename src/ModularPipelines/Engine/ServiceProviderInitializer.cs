using Initialization.Microsoft.Extensions.DependencyInjection.Extensions;

namespace ModularPipelines.Engine;

internal class ServiceProviderInitializer(IServiceProvider serviceProvider) : IServiceProviderInitializer
{
    public async Task InitializeAsync()
    {
        await serviceProvider.InitializeAsync();
    }
}
