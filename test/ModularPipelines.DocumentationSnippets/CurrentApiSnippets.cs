using ModularPipelines.Secrets;
using EnumerableAsyncProcessor.Extensions;
using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Attributes;
using ModularPipelines;
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

        builder.ConfigureOptions(options => options with
        {
            Secrets = options.Secrets with
            {
                CaseInsensitive = true,
                MinimumSecretLength = 4,
                MaskValue = "[REDACTED]",
            },
        });

        return builder.RunAsync();
    }

    public static async Task<int> RunPipelineAndMapExitCode(string[] args)
    {
        var builder = Pipeline.CreateBuilder(args);
        builder.ConfigureOptions(options => options with
        {
            FailureMode = FailureMode.ContinueOnFailure,
            ThrowOnPipelineFailure = false,
        });
        builder.AddModule<BuildModule>();

        await using var pipeline = await builder.BuildAsync();
        var summary = await pipeline.RunAsync();

        return summary.Status == ModuleStatus.Failed ? 1 : 0;
    }

    public static async Task VerifySuccessfulPipeline(string[] args)
    {
        var builder = Pipeline.CreateBuilder(args);
        builder.ConfigureOptions(options => options with
        {
            ThrowOnPipelineFailure = false,
        });
        builder.AddModule<BuildModule>();

        var result = await builder.RunAsync();
        if (result.Status != ModuleStatus.Succeeded)
        {
            throw new InvalidOperationException("Expected a successful pipeline.");
        }
    }

    public static async Task ConfigureSingleFilePipeline(string[] args)
    {
        var builder = Pipeline.CreateBuilder(args);
        builder
            .AddModule<UpdateDotnetWorkloads>()
            .AddModule<CheckDotnetSdkModule>();

        await builder.RunAsync();
    }

    public static async Task ConfigureTestPackPublishPipeline(string[] args)
    {
        var builder = Pipeline.CreateBuilder(args);
        builder
            .AddModule<NugetVersionGeneratorModule>()
            .AddModule<RunUnitTestsModule>()
            .AddModule<PackProjectsModule>()
            .AddModule<UploadPackagesToNugetModule>();

        await builder.RunAsync();
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

    public sealed class UpdateDotnetWorkloads : Module<CommandResult>
    {
        protected override async Task<CommandResult> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            return await context.Tools.DotNet.Workload
                .UpdateAsync(cancellationToken: cancellationToken);
        }
    }

    [DependsOn<UpdateDotnetWorkloads>]
    public sealed class CheckDotnetSdkModule : Module<CommandResult>
    {
        protected override async Task<CommandResult> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            return await context.Tools.DotNet.Sdk
                .CheckAsync(cancellationToken: cancellationToken);
        }
    }

    public sealed class NugetVersionGeneratorModule : Module<string>
    {
        protected override async Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            var version = await context.Tools.Git.Versioning.GetVersioningInformationAsync(cancellationToken);
            return version.FullSemVer
                   ?? throw new InvalidOperationException(
                       "GitVersion did not return a semantic version.");
        }
    }

    public sealed class RunUnitTestsModule : Module<CommandResult[]>
    {
        protected override async Task<CommandResult[]> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            var repository = await context.Tools.Git.Information.GetInfoAsync(cancellationToken)
                             ?? throw new InvalidOperationException(
                                 "Git repository information is unavailable.");
            var testProjects = repository.Root
                .GetFiles(file => file.Name.EndsWith(
                    ".UnitTests.csproj",
                    StringComparison.OrdinalIgnoreCase))
                .ToList();
            return await testProjects
                .ToAsyncProcessorBuilder()
                .SelectAsync(testProject => context.Tools.DotNet.TestAsync(
                    new DotNetTestOptions
                    {
                        Project = testProject.Path,
                        Arguments =
                        [
                            "--coverage",
                            "--coverage-output-format", "cobertura",
                        ],
                    },
                    cancellationToken: cancellationToken))
                .ProcessInParallel();
        }
    }

    [DependsOn<NugetVersionGeneratorModule>]
    [DependsOn<RunUnitTestsModule>]
    public sealed class PackProjectsModule : Module<CommandResult[]>
    {
        protected override async Task<CommandResult[]> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            var packageVersion = await context.GetModule<NugetVersionGeneratorModule>();
            var repository = await context.Tools.Git.Information.GetInfoAsync(cancellationToken)
                             ?? throw new InvalidOperationException(
                                 "Git repository information is unavailable.");
            var projects = repository.Root
                .GetFiles(file => file.Extension == ".csproj"
                                  && !file.Name.EndsWith(
                                      ".UnitTests.csproj",
                                      StringComparison.OrdinalIgnoreCase))
                .ToList();
            var packageDirectory = repository.Root
                .GetFolder("artifacts")
                .GetFolder("packages");
            return await projects
                .ToAsyncProcessorBuilder()
                .SelectAsync(project => context.Tools.DotNet.PackAsync(
                    new DotNetPackOptions
                    {
                        ProjectSolution = project.Path,
                        Output = packageDirectory.Path,
                        IncludeSource = true,
                        Properties =
                        [
                            new KeyValue("PackageVersion", packageVersion.Value),
                            new KeyValue("Version", packageVersion.Value),
                        ],
                    },
                    cancellationToken: cancellationToken))
                .ProcessInParallel();
        }
    }

    [DependsOn<PackProjectsModule>]
    public sealed class UploadPackagesToNugetModule : Module<CommandResult[]>
    {
        protected override async Task<CommandResult[]> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            var apiKey = context.Environment.Variables.Get("NUGET_API_KEY");
            ArgumentException.ThrowIfNullOrWhiteSpace(apiKey);

            var repository = await context.Tools.Git.Information.GetInfoAsync(cancellationToken)
                             ?? throw new InvalidOperationException(
                                 "Git repository information is unavailable.");
            var packages = repository.Root
                .GetFolder("artifacts")
                .GetFolder("packages")
                .GetFiles(file => file.Extension == ".nupkg"
                                  && !file.Name.EndsWith(
                                      ".symbols.nupkg",
                                      StringComparison.OrdinalIgnoreCase))
                .ToList();
            return await packages
                .ToAsyncProcessorBuilder()
                .SelectAsync(package => context.Tools.DotNet.NuGet.PushAsync(
                    new DotNetNuGetPushOptions
                    {
                        Path = package.Path,
                        Source = "https://api.nuget.org/v3/index.json",
                        ApiKey = apiKey,
                    },
                    cancellationToken: cancellationToken))
                .ProcessOneAtATime();
        }
    }

    public sealed class HasDotNetSdkRequirement : PipelineRequirement
    {
        public override async Task<RequirementDecision> EvaluateAsync(
            IPipelineContext context,
            CancellationToken cancellationToken)
        {
            var result = await context.Shell.RunAsync(
                    "dotnet",
                    ["--version"],
                    cancellationToken: cancellationToken)
                .ConfigureAwait(false);

            return result.ExitCode == 0 ? Pass() : Fail(".NET SDK is not installed");
        }
    }

    public sealed class HasGitHubToken : IRunCondition
    {
        public Task<bool> EvaluateAsync(IPipelineContext context)
            => Task.FromResult(!string.IsNullOrEmpty(
                context.Environment.Variables.Get("GITHUB_TOKEN")));
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
