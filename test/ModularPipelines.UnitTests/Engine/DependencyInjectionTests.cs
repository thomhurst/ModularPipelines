using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ModularPipelines.Attributes;
using ModularPipelines.Cmd;
using ModularPipelines.Cmd.Extensions;
using ModularPipelines.DependencyInjection;
using ModularPipelines.DotNet.Extensions;
using ModularPipelines.DotNet.Services;
using ModularPipelines.Git;
using ModularPipelines.Git.Extensions;
using ModularPipelines.TestHelpers;
using ModularPipelines.UnitTests.Modules;
using Moq;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace ModularPipelines.UnitTests.Engine;

public class DependencyInjectionTests
{
    [Test]
    public async Task AllDependenciesCanBeBuilt()
    {
        var host = await TestPipelineHostBuilder.Create()
            .AddModule<TestModule1>()
            .BuildHostAsync();

        var services = host.Services;

        var collection = services.GetRequiredService<IPipelineServiceContainerWrapper>().ServiceCollection;

        foreach (var serviceDescriptor in collection.Where(sd => sd.ServiceType.Namespace?.StartsWith("ModularPipeline") == true && !sd.ServiceType.IsGenericType))
        {
            services.GetRequiredService(serviceDescriptor.ServiceType);
        }
    }

    [Test]
    public async Task GeneratedIntegrationMetadataRegistersEveryPipeline()
    {
        var integrationAssemblies = new[]
        {
            typeof(CmdExtensions).Assembly,
            typeof(DotNetExtensions).Assembly,
            typeof(GitExtensions).Assembly,
        };

        foreach (var integrationAssembly in integrationAssemblies)
        {
            await Assert.That(
                    integrationAssembly.GetCustomAttributes<ModularPipelinesContextAttribute>())
                .HasSingleItem();
        }

        for (var i = 0; i < 2; i++)
        {
            await using var pipeline = await TestPipelineHostBuilder.Create()
                .AddModule<TestModule1>()
                .BuildHostAsync();

            pipeline.Services.GetRequiredService<ICmd>();
            pipeline.Services.GetRequiredService<IDotNet>();
            pipeline.Services.GetRequiredService<IGit>();
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
