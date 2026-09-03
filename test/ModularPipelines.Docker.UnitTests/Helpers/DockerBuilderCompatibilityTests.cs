#pragma warning disable CS0618 // Tests intentionally verify obsolete compatibility aliases.

using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Context;
using ModularPipelines.Context.Domains.Shell;
using ModularPipelines.Docker.Enums;
using ModularPipelines.Docker.Extensions;
using ModularPipelines.Docker.Options;
using ModularPipelines.Docker.Services;
using ModularPipelines.Helpers.Internal;
using ModularPipelines.Options;
using static ModularPipelines.TestHelpers.OptionsRenderingTestHelper;

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

        await Assert.That(typeof(IDockerBuilder).GetMethod("BuildAsync", parameterTypes))
            .IsNotNull();
        await Assert.That(typeof(DockerBuilder).GetMethod("BuildAsync", parameterTypes))
            .IsNotNull();
        var rootParameterTypes = new[]
        {
            typeof(DockerBuilderOptions),
            typeof(CommandExecutionOptions),
            typeof(CancellationToken),
        };
        await Assert.That(typeof(IDockerBuilder).GetMethod("ExecuteAsync", rootParameterTypes))
            .IsNotNull();
        await Assert.That(typeof(DockerBuilder).GetMethod("ExecuteAsync", rootParameterTypes))
            .IsNotNull();
        await Assert.That(typeof(IDockerBuilder).GetProperty(nameof(IDockerBuilder.History))!.PropertyType)
            .IsEqualTo(typeof(DockerBuilderHistory));
        await Assert.That(typeof(DockerBuilderHistory).GetProperty(
                nameof(DockerBuilderHistory.Inspect),
                BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)!.PropertyType)
            .IsEqualTo(typeof(DockerBuilderHistoryInspect));

        var nestedMethods = new (Type ServiceType, string MethodName, Type OptionsType)[]
        {
            (typeof(DockerBuilderDap), "BuildAsync", typeof(DockerBuilderDapBuildOptions)),
            (typeof(DockerBuilderHistory), "ExportAsync", typeof(DockerBuilderHistoryExportOptions)),
            (typeof(DockerBuilderHistory), "ImportAsync", typeof(DockerBuilderHistoryImportOptions)),
            (typeof(DockerBuilderHistory), "LogsAsync", typeof(DockerBuilderHistoryLogsOptions)),
            (typeof(DockerBuilderHistory), "LsAsync", typeof(DockerBuilderHistoryLsOptions)),
            (typeof(DockerBuilderHistory), "OpenAsync", typeof(DockerBuilderHistoryOpenOptions)),
            (typeof(DockerBuilderHistory), "RmAsync", typeof(DockerBuilderHistoryRmOptions)),
            (typeof(DockerBuilderHistory), "TraceAsync", typeof(DockerBuilderHistoryTraceOptions)),
            (typeof(DockerBuilderHistoryInspect), "AttachmentAsync", typeof(DockerBuilderHistoryInspectAttachmentOptions)),
            (typeof(DockerBuilderImageTools), "CreateAsync", typeof(DockerBuilderImageToolsCreateOptions)),
            (typeof(DockerBuilderImageTools), "InspectAsync", typeof(DockerBuilderImageToolsInspectOptions)),
            (typeof(DockerBuilderPolicy), "EvalAsync", typeof(DockerBuilderPolicyEvalOptions)),
            (typeof(DockerBuilderPolicy), "TestAsync", typeof(DockerBuilderPolicyTestOptions)),
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
    public async Task BuilderEnumOptionPreservesLegacyTypeAndCanonicalValue()
    {
        var options = new DockerBuilderHistoryLogsOptions
        {
            Progress = DockerBuilderHistoryLogsProgress.Plain,
        };
        var canonicalOptions = (DockerBuildxHistoryLogsOptions) options;

        await Assert.That(options.Progress)
            .IsEqualTo(DockerBuilderHistoryLogsProgress.Plain);
        await Assert.That(canonicalOptions.Progress)
            .IsEqualTo(DockerBuildxHistoryLogsProgress.Plain);

        canonicalOptions.Progress = DockerBuildxHistoryLogsProgress.Rawjson;

        await Assert.That(options.Progress)
            .IsEqualTo(DockerBuilderHistoryLogsProgress.Rawjson);
    }

    [Test]
    public async Task BuilderEnumOptionRendersCanonicalSwitch()
    {
        var options = new DockerBuilderHistoryLogsOptions
        {
            Progress = DockerBuilderHistoryLogsProgress.Plain,
        };
        var model = new CommandModelProvider()
            .GetCommandModel(typeof(DockerBuilderHistoryLogsOptions));
        var arguments = new CommandArgumentBuilder().BuildArguments(model, options);

        await AssertArguments(arguments, ["--progress=plain"]);
    }

    [Test]
    [Arguments(typeof(DockerBuilderBuildOptions), "context")]
    [Arguments(typeof(DockerBuilderDapBuildOptions), "context")]
    [Arguments(typeof(DockerBuilderPolicyEvalOptions), "policy.json")]
    [Arguments(typeof(DockerBuilderPolicyTestOptions), "policy.json")]
    public async Task BuilderOptionsRenderInheritedPositionalOperand(Type optionsType, string operand)
    {
        var options = Activator.CreateInstance(optionsType, operand)!;
        var model = new CommandModelProvider().GetCommandModel(optionsType);
        var arguments = new CommandArgumentBuilder().BuildArguments(model, options);

        await Assert.That(arguments).Contains(operand);
    }

    [Test]
    public async Task ComposeExecNoTtyRendersCanonicalSwitch()
    {
        var options = new DockerComposeExecOptions("service", "command")
        {
            NoTty = true,
        };
        var model = new CommandModelProvider()
            .GetCommandModel(typeof(DockerComposeExecOptions));
        var arguments = new CommandArgumentBuilder().BuildArguments(model, options);

        await Assert.That(arguments).Contains("--no-TTY=true");
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
