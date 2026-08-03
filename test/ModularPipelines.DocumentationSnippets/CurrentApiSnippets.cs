using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Attributes;
using ModularPipelines.Conditions;
using ModularPipelines.Context;
using ModularPipelines.DotNet.Options;
using ModularPipelines.Enums;
using ModularPipelines.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using ModularPipelines.Requirements;

namespace ModularPipelines.DocumentationSnippets;

// These types mirror the public snippets. This assembly deliberately is not a friend of
// ModularPipelines, so it compiles against the same public surface that consumers use.
public static class CurrentApiSnippets
{
    public static Task ConfigurePipeline(string[] args)
    {
        var builder = Pipeline.CreateBuilder(args);

        builder
            .AddModule<BuildModule>()
            .AddModule<TestModule>()
            .AddModule<PublishModule>()
            .AddModule<PublishCommandModule>()
            .AddModule<ArtifactPathResolver>()
            .AddRequirement<HasDotNetSdkRequirement>();

        builder.Services.Configure<SecretMaskingOptions>(options =>
        {
            options.CaseInsensitive = true;
            options.MinimumSecretLength = 4;
            options.MaskValue = "[REDACTED]";
        });

        return builder.ExecutePipelineAsync();
    }

    public static async Task<int> RunPipelineAndMapExitCode(string[] args)
    {
        using var builder = Pipeline.CreateBuilder(args);
        builder.ConfigurePipelineOptions(options => options with
        {
            ExecutionMode = ExecutionMode.WaitForAllModules,
            ThrowOnPipelineFailure = false,
        });
        builder.AddModule<BuildModule>();

        await using var pipeline = await builder.BuildAsync();
        var summary = await pipeline.RunAsync();

        return summary.Status == Status.Failed ? 1 : 0;
    }

    public static async Task VerifySuccessfulPipeline(string[] args)
    {
        using var builder = Pipeline.CreateBuilder(args);
        builder.ConfigurePipelineOptions(options => options with
        {
            ThrowOnPipelineFailure = false,
        });
        builder.AddModule<BuildModule>();

        var result = await builder.ExecutePipelineAsync();
        if (result.Status != Status.Successful)
        {
            throw new InvalidOperationException("Expected a successful pipeline.");
        }
    }

    public sealed record BuildInfo(string Version, string OutputPath);

    public sealed class BuildModule : Module<BuildInfo>
    {
        protected override async Task<BuildInfo> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            await context.Tools.DotNet.BuildAsync(
                new DotNetBuildOptions
                {
                    ProjectSolution = "MySolution.sln",
                    Configuration = "Release",
                },
                cancellationToken: cancellationToken).ConfigureAwait(false);

            return new BuildInfo("1.0.0", "bin/Release");
        }
    }

    [DependsOn<BuildModule>]
    public sealed class TestModule : Module<CommandResult>
    {
        protected override async Task<CommandResult> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            return await context.Tools.DotNet.TestAsync(
                new DotNetTestOptions
                {
                    Project = "MySolution.sln",
                    Configuration = "Release",
                },
                cancellationToken: cancellationToken).ConfigureAwait(false);
        }
    }

    [DependsOn<BuildModule>]
    public sealed class PublishModule : Module
    {
        protected override async Task ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            var buildResult = await context.GetModule<BuildModule>();
            _ = buildResult.Value.OutputPath;
        }
    }

    public sealed class PublishCommandModule : Module<CommandResult>
    {
        protected override async Task<CommandResult> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            return await context.Tools.DotNet.PublishAsync(
                new DotNetPublishOptions
                {
                    ProjectSolution = "src/MyApp/MyApp.csproj",
                    Configuration = "Release",
                    Output = "publish/",
                },
                cancellationToken: cancellationToken).ConfigureAwait(false);
        }
    }

    public sealed class HasDotNetSdkRequirement : PipelineRequirement
    {
        public override async Task<RequirementDecision> MustAsync(IPipelineContext context)
        {
            var result = await context.Shell.Command.ExecuteCommandLineToolAsync(
                new GenericCommandLineToolOptions("dotnet") { Arguments = ["--version"] })
                .ConfigureAwait(false);

            return result.ExitCode == 0 ? Pass() : Fail(".NET SDK is not installed");
        }
    }

    public sealed class HasGitHubToken : IRunCondition
    {
        public Task<bool> EvaluateAsync(IPipelineContext context)
            => Task.FromResult(!string.IsNullOrEmpty(
                context.Environment.Variables.GetEnvironmentVariable("GITHUB_TOKEN")));
    }

    public sealed class VersionCalculator : SyncModule<string>
    {
        protected override string Execute(IModuleContext context, CancellationToken cancellationToken)
            => "1.0.0";
    }

    [DependsOn<BuildModule>]
    public sealed class ArtifactPathResolver : Module<string>
    {
        protected override async Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            var buildResult = await context.GetModule<BuildModule>();
            return Path.Combine(buildResult.Value.OutputPath, "artifacts");
        }
    }
}
