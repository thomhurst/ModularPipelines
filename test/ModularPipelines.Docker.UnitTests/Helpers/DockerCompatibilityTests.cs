using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Context;
using ModularPipelines.Docker.Extensions;
using ModularPipelines.Docker.Options;
using ModularPipelines.Docker.Services;
using ModularPipelines.TestHelpers;
using static ModularPipelines.TestHelpers.OptionsRenderingTestHelper;

namespace ModularPipelines.Docker.UnitTests.Helpers;

public class DockerCompatibilityTests : TestBase
{
    [Test]
    public async Task ClientOptionsRenderBeforeCommand()
    {
        var builder = await GetService<ICommandLineBuilder>();
        var arguments = builder.Build(new DockerInfoOptions
        {
            Context = "remote",
            Debug = true,
            Host = ["tcp://first:2376", "tcp://second:2376"],
            Tlsverify = true,
        }).Arguments;

        await AssertArguments(arguments,
            ["--context=remote", "--debug", "--host=tcp://first:2376", "--host=tcp://second:2376", "--tlsverify", "info"]);
    }

    [Test]
    public async Task ClientContextAndBuildxOperandRemainIndependent()
    {
        var builder = await GetService<ICommandLineBuilder>();
        var arguments = builder.Build(new DockerBuildxCreateOptions
        {
            Context = "client-context",
            Debug = true,
            BuildxDebug = true,
            BuildxContext = "builder-context",
        }).Arguments;

        await AssertArguments(arguments,
            ["--context=client-context", "--debug", "buildx", "create", "--debug", "builder-context"]);
    }

    [Test]
    public async Task ClientContextDoesNotReplaceRequiredContextOperands()
    {
        var builder = await GetService<ICommandLineBuilder>();
        var arguments = builder.Build(new DockerContextRmOptions(["first", "second"])
        {
            Context = "client-context",
        }).Arguments;

        await AssertArguments(arguments, ["--context=client-context", "context", "rm", "first", "second"]);
    }

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
