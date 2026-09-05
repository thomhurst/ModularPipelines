using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Context;
using ModularPipelines.Context.Domains.Shell;
using ModularPipelines.Docker.Extensions;
using ModularPipelines.Docker.Options;
using ModularPipelines.Docker.Services;
using static ModularPipelines.TestHelpers.OptionsRenderingTestHelper;

namespace ModularPipelines.Docker.UnitTests.Helpers;

public class DockerCompatibilityTests
{
    [Test]
    public async Task ComposeExecNoTtyRendersCanonicalSwitch()
    {
        var arguments = BuildArguments(new DockerComposeExecOptions("service", "command")
        {
            NoTty = true,
        });

        await Assert.That(arguments).Contains("--no-TTY=true");
    }

    [Test]
    public async Task CustomBuildxRegistrationResolvesFromDocker()
    {
        var customBuildx = DispatchProxy.Create<IDockerBuildx, ThrowingProxy>();
        var command = DispatchProxy.Create<ICommandContext, ThrowingProxy>();
        var services = new ServiceCollection();
        services.AddScoped(_ => customBuildx);
        services.AddScoped(_ => command);
        services.RegisterDockerContext();

        await using var provider = services.BuildServiceProvider();
        await using var scope = provider.CreateAsyncScope();

        var resolvedBuildx = scope.ServiceProvider.GetRequiredService<IDockerBuildx>();
        var docker = scope.ServiceProvider.GetRequiredService<IDocker>();

        await Assert.That(resolvedBuildx).IsSameReferenceAs(customBuildx);
        await Assert.That(docker.Buildx).IsSameReferenceAs(customBuildx);
    }

    private class ThrowingProxy : DispatchProxy
    {
        protected override object? Invoke(
            MethodInfo? targetMethod,
            object?[]? args) =>
            throw new InvalidOperationException(
                $"Unexpected invocation of {targetMethod?.Name}.");
    }
}
