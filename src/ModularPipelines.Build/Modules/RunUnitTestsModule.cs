using EnumerableAsyncProcessor.Extensions;
using Microsoft.Extensions.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Build.Settings;
using ModularPipelines.Configuration;
using ModularPipelines.Context;
using ModularPipelines.DotNet.Extensions;
using ModularPipelines.DotNet.Options;
using ModularPipelines.Git.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using Polly;

namespace ModularPipelines.Build.Modules;

[DependsOn<BuildSolutionsModule>(Optional = true)]
[ConsumesArtifact(typeof(BuildSolutionsModule), "build-output", RestorePath = "../../")]
public class RunUnitTestsModule : Module<CommandResult[]>
{
    private readonly IOptions<PipelineSettings> _pipelineSettings;

    public RunUnitTestsModule(IOptions<PipelineSettings> pipelineSettings)
    {
        _pipelineSettings = pipelineSettings;
    }

    protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
        .WithRetryPolicy(Policy.Handle<Exception>().RetryAsync(0))
        .Build();

    protected override async Task<CommandResult[]?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        return await context.Git().RootDirectory
            .GetFiles(file => file.Path.EndsWith(".csproj", StringComparison.OrdinalIgnoreCase)
                              && file.Path.Contains("UnitTests", StringComparison.OrdinalIgnoreCase))
            .ToAsyncProcessorBuilder()
            .SelectAsync(async unitTestProjectFile => await context.DotNet().Run(new DotNetRunOptions
            {
                Project = unitTestProjectFile.Path,
                NoBuild = true,
                Framework = _pipelineSettings.Value.TestFramework,
                Arguments = ["--coverage", "--coverage-output-format", "cobertura", "--hangdump", "--hangdump-timeout", "20m"],
                Configuration = "Release",
                Properties =
                [
                    new("RunAnalyzersDuringBuild", "false"),
                    new("RunAnalyzers", "false")
                ],
            },
            new CommandExecutionOptions
            {
                EnvironmentVariables = new Dictionary<string, string?>
                {
                    ["GITHUB_ACTIONS"] = null,
                    ["GITHUB_STEP_SUMMARY"] = null,
                },
            },
            cancellationToken))
            .ProcessInParallel();
    }
}
