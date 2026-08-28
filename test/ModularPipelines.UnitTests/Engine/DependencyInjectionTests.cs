using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ModularPipelines.Attributes;
using ModularPipelines.Cmd;
using ModularPipelines.Cmd.Extensions;
using ModularPipelines.DependencyInjection;
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
        var host = await TestPipelineBuilder.Create()
            .AddModule<TestModule1>()
            .BuildAsync();

        var services = host.Services;

        var collection = services.GetRequiredService<IPipelineServiceContainerWrapper>().ServiceCollection;

        foreach (var serviceDescriptor in collection.Where(sd => sd.ServiceType.Namespace?.StartsWith("ModularPipeline") == true && !sd.ServiceType.IsGenericType))
        {
            services.GetRequiredService(serviceDescriptor.ServiceType);
        }
    }

    [Test]
    public async Task GeneratedCmdIntegrationMetadataRegistersEveryPipeline()
    {
        await Assert.That(
                typeof(CmdExtensions).Assembly
                    .GetCustomAttributes<ModularPipelinesContextAttribute>())
            .HasSingleItem();

        for (var i = 0; i < 2; i++)
        {
            await using var pipeline = await TestPipelineBuilder.Create()
                .AddModule<TestModule1>()
                .BuildAsync();

            pipeline.Services.GetRequiredService<ICmdContext>();
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

        using var serviceProvider = serviceCollection.BuildServiceProvider(new ServiceProviderOptions
        {
            ValidateScopes = true,
            ValidateOnBuild = true,
        });

        serviceProvider.GetRequiredService<global::ModularPipelines.PipelineCli.PipelineCommandHandler>();
    }
}
