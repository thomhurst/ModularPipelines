using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ModularPipelines.DependencyInjection;
using ModularPipelines.TestHelpers;
using Moq;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

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

    [Test]
    public void Validate()
    {
        var serviceCollection = new ServiceCollection()
            .AddSingleton(Mock.Of<IHost>())
            .AddSingleton(Mock.Of<IHostEnvironment>())
            .AddSingleton(Mock.Of<IConfiguration>());

        DependencyInjectionSetup.Initialize(serviceCollection);

        serviceCollection.BuildServiceProvider(new ServiceProviderOptions
        {
            ValidateScopes = true,
            ValidateOnBuild = true,
        });
    }
}