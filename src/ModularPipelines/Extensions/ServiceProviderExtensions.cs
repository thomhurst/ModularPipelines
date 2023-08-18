using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.DependencyInjection;
using TomLonghurst.EnumerableAsyncProcessor.Extensions;
using TomLonghurst.Microsoft.Extensions.DependencyInjection.ServiceInitialization;

namespace ModularPipelines.Extensions;

internal static class ServiceProviderExtensions
{
    public static async Task<AsyncServiceScope> CreateInitializedAsyncScope(this IServiceProvider serviceProvider)
    {
        var scope = serviceProvider.CreateAsyncScope();

        var initializers = serviceProvider.GetRequiredService<IPipelineServiceContainerWrapper>()
            .ServiceCollection
            .Where(t => t.ImplementationType?.IsAssignableTo(typeof(IInitializer)) == true)
            .Select(x => x.ServiceType)
            .ToList();

        await initializers
            .ToAsyncProcessorBuilder()
            .ForEachAsync(async x =>
            {
                var initializer = (IInitializer) scope.ServiceProvider.GetRequiredService(x);
                await initializer.InitializeAsync();
            })
            .ProcessInParallel();
        
        return scope;
    }
}