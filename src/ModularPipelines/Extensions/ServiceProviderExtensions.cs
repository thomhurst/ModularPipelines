using Microsoft.Extensions.DependencyInjection;
using TomLonghurst.Microsoft.Extensions.DependencyInjection.ServiceInitialization.Extensions;

namespace ModularPipelines.Extensions;

internal static class ServiceProviderExtensions
{
    public static async Task<AsyncServiceScope> CreateInitializedAsyncScope(this IServiceProvider serviceProvider)
    {
        var scope = serviceProvider.CreateAsyncScope();

        await scope.ServiceProvider.InitializeAsync();
        
        return scope;
    }
}