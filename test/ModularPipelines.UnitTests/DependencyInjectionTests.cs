using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.Internal;
using ModularPipelines.Cmd.Extensions;
using ModularPipelines.DependencyInjection;
using ModularPipelines.Docker.Extensions;
using ModularPipelines.DotNet.Extensions;
using ModularPipelines.Git.Extensions;
using ModularPipelines.Helm.Extensions;
using ModularPipelines.Host;
using ModularPipelines.Kubernetes.Extensions;
using ModularPipelines.MicrosoftTeams.Extensions;
using ModularPipelines.Node.Extensions;
using ModularPipelines.NuGet.Extensions;
using ModularPipelines.Terraform.Extensions;
using Moq;

namespace ModularPipelines.UnitTests;

public class DependencyInjectionTests
{
    [Test]
    public void AllDependenciesCanBeBuilt()
    {
        var services = PipelineHostBuilder.Create()
            .ConfigureServices((context, serviceCollection) => RegisterOptionalContexts(serviceCollection))
            .BuildHost()
            .Services;

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

        RegisterOptionalContexts(services);
        
        services.BuildServiceProvider(new ServiceProviderOptions
        {
            ValidateScopes = true,
            ValidateOnBuild = true
        });
    }

    private static void RegisterOptionalContexts(IServiceCollection services)
    {
        services.RegisterGitContext()
            .RegisterCmdContext()
            .RegisterDockerContext()
            .RegisterHelmContext()
            .RegisterNodeContext()
            .RegisterKubernetesContext()
            .RegisterTerraformContext()
            .RegisterDotNetContext()
            .RegisterMicrosoftTeamsContext()
            .RegisterNuGetContext();
    }
}