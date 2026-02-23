using Microsoft.Extensions.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Build.Settings;
using ModularPipelines.Context;
using ModularPipelines.DotNet.Extensions;
using ModularPipelines.DotNet.Options;
using ModularPipelines.Git.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Configuration;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using Polly;

namespace ModularPipelines.Build.Modules.UnitTests;

[DependsOn<BuildSolutionsModule>(Optional = true)]
[ConsumesArtifact(typeof(BuildSolutionsModule), "build-output", RestorePath = "../../")]
[RequiresCapability("linux")]
public abstract class RunUnitTestModule : Module<CommandResult>
{
    private readonly IOptions<PipelineSettings> _pipelineSettings;

    protected RunUnitTestModule(IOptions<PipelineSettings> pipelineSettings)
    {
        _pipelineSettings = pipelineSettings;
    }

    protected abstract string TestProjectFileName { get; }

    protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
        .WithRetryPolicy(Policy.Handle<Exception>().RetryAsync(0))
        .Build();

    protected override async Task<CommandResult?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        var testProject = context.Git().RootDirectory
            .GetFiles(file => file.Name.Equals(TestProjectFileName, StringComparison.OrdinalIgnoreCase))
            .Single();

        return await context.DotNet().Run(new DotNetRunOptions
        {
            Project = testProject.Path,
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
                // Clear distributed mode env vars to prevent test subprocesses
                // from inheriting coordinator/artifact store connections
                ["INSTANCE_INDEX"] = null,
                ["TOTAL_INSTANCES"] = null,
                ["UPSTASH_REDIS_REST_URL"] = null,
                ["UPSTASH_REDIS_REST_TOKEN"] = null,
                ["R2_ENDPOINT_URL"] = null,
                ["R2_ACCESS_KEY"] = null,
                ["R2_SECRET_KEY"] = null,
            },
        },
        cancellationToken);
    }
}
