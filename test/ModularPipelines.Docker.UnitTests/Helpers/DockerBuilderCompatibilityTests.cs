#pragma warning disable CS0618 // Tests intentionally verify obsolete compatibility aliases.

using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Context.Domains.Shell;
using ModularPipelines.Docker.Extensions;
using ModularPipelines.Docker.Options;
using ModularPipelines.Docker.Services;
using ModularPipelines.Options;

namespace ModularPipelines.Docker.UnitTests.Helpers;

public class DockerBuilderCompatibilityTests
{
    [Test]
    public async Task BuilderApiPreservesLegacySignatures()
    {
        var parameterTypes = new[]
        {
            typeof(DockerBuilderBuildOptions),
            typeof(CommandExecutionOptions),
            typeof(CancellationToken),
        };

        await Assert.That(typeof(IDockerBuilder).GetMethod("Build", parameterTypes))
            .IsNotNull();
        await Assert.That(typeof(DockerBuilder).GetMethod("Build", parameterTypes))
            .IsNotNull();
        await Assert.That(typeof(IDockerBuilder).GetProperty(nameof(IDockerBuilder.History))!.PropertyType)
            .IsEqualTo(typeof(DockerBuilderHistory));
        await Assert.That(typeof(DockerBuilderHistory).GetProperty(
                nameof(DockerBuilderHistory.Inspect),
                BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)!.PropertyType)
            .IsEqualTo(typeof(DockerBuilderHistoryInspect));

        var nestedMethods = new (Type ServiceType, string MethodName, Type OptionsType)[]
        {
            (typeof(DockerBuilderDap), "Build", typeof(DockerBuilderDapBuildOptions)),
            (typeof(DockerBuilderHistory), "Export", typeof(DockerBuilderHistoryExportOptions)),
            (typeof(DockerBuilderHistory), "Import", typeof(DockerBuilderHistoryImportOptions)),
            (typeof(DockerBuilderHistory), "Logs", typeof(DockerBuilderHistoryLogsOptions)),
            (typeof(DockerBuilderHistory), "Ls", typeof(DockerBuilderHistoryLsOptions)),
            (typeof(DockerBuilderHistory), "Open", typeof(DockerBuilderHistoryOpenOptions)),
            (typeof(DockerBuilderHistory), "Rm", typeof(DockerBuilderHistoryRmOptions)),
            (typeof(DockerBuilderHistory), "Trace", typeof(DockerBuilderHistoryTraceOptions)),
            (typeof(DockerBuilderHistoryInspect), "Attachment", typeof(DockerBuilderHistoryInspectAttachmentOptions)),
            (typeof(DockerBuilderImageTools), "Create", typeof(DockerBuilderImageToolsCreateOptions)),
            (typeof(DockerBuilderImageTools), "Inspect", typeof(DockerBuilderImageToolsInspectOptions)),
            (typeof(DockerBuilderPolicy), "Eval", typeof(DockerBuilderPolicyEvalOptions)),
            (typeof(DockerBuilderPolicy), "Test", typeof(DockerBuilderPolicyTestOptions)),
        };

        foreach (var (serviceType, methodName, optionsType) in nestedMethods)
        {
            await Assert.That(serviceType.GetMethod(
                    methodName,
                    [optionsType, typeof(CommandExecutionOptions), typeof(CancellationToken)]))
                .IsNotNull();
        }
    }

    [Test]
    public async Task CustomBuildxRegistrationDoesNotNeedToImplementBuilder()
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
        var resolvedBuilder = scope.ServiceProvider.GetRequiredService<IDockerBuilder>();
        var docker = scope.ServiceProvider.GetRequiredService<IDocker>();

        await Assert.That(resolvedBuildx).IsSameReferenceAs(customBuildx);
        await Assert.That(resolvedBuilder).IsAssignableTo<DockerBuilder>();
        await Assert.That(docker.Buildx).IsSameReferenceAs(customBuildx);
        await Assert.That(docker.Builder).IsSameReferenceAs(resolvedBuilder);
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
