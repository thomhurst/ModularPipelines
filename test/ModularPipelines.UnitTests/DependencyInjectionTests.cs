using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.DependencyInjection;

namespace ModularPipelines.UnitTests;

public class DependencyInjectionTests
{
    [Test]
    public async Task AllDependenciesCanBeBuilt()
    {
        var host = await TestPipelineHostBuilder.Create().BuildHostAsync();
        
        var services = host.Services;

        var collection = services.GetRequiredService<IPipelineServiceContainerWrapper>().ServiceCollection;

        foreach (var serviceDescriptor in collection.Where(sd => sd.ServiceType.Namespace?.StartsWith("ModularPipeline") == true && !sd.ServiceType.IsGenericType))
        {
            services.GetRequiredService(serviceDescriptor.ServiceType);
        }
    }
}