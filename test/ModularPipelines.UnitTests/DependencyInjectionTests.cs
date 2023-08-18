using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.DependencyInjection;
using ModularPipelines.Host;
using ModularPipelines.Options;

namespace ModularPipelines.UnitTests;

public class DependencyInjectionTests
{
    [Test]
    public void AllDependenciesCanBeBuilt()
    {
        var services = PipelineHostBuilder.Create()
            .ConfigureServices((context, collection) => collection.Configure<PipelineOptions>(opt => opt.ShowProgressInConsole = false))
            .BuildHost().Services;

        var collection = services.GetRequiredService<IPipelineServiceContainerWrapper>().ServiceCollection;

        foreach (var serviceDescriptor in collection.Where(sd => sd.ServiceType.Namespace?.StartsWith("ModularPipeline") == true && !sd.ServiceType.IsGenericType))
        {
            services.GetRequiredService(serviceDescriptor.ServiceType);
        }
    }
}