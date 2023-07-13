using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.DependencyInjection;
using ModularPipelines.Host;

namespace ModularPipelines.UnitTests;

public class DependencyInjectionTests
{
    [Test]
    public void AllDependenciesCanBeBuilt()
    {
        var services = PipelineHostBuilder.Create().BuildHost().Services;

        var collection = services.GetRequiredService<IPipelineServiceContainerWrapper>().ServiceCollection;
        
        foreach (var serviceDescriptor in collection.Where(sd => sd.ServiceType.Namespace?.StartsWith("ModularPipeline") == true && !sd.ServiceType.IsGenericType))
        {
            services.GetRequiredService(serviceDescriptor.ServiceType);
        }
    }
}