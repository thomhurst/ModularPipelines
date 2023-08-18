using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.Internal;
using ModularPipelines.DependencyInjection;
using ModularPipelines.Host;
using Moq;

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

    [Test]
    public void Lifecycles()
    {
        var services = new ServiceCollection()
            .AddSingleton<IHostEnvironment>(new HostingEnvironment())
            .AddSingleton<IConfiguration>(new Mock<IConfiguration>().Object);
        
        DependencyInjectionSetup.Initialize(services);

        services.BuildServiceProvider(new ServiceProviderOptions
        {
            ValidateScopes = true,
            ValidateOnBuild = true
        });
    }
}