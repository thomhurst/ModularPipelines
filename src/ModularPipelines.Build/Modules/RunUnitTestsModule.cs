using EnumerableAsyncProcessor.Extensions;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.DotNet.Extensions;
using ModularPipelines.DotNet.Options;
using ModularPipelines.Git.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using Polly.Retry;

namespace ModularPipelines.Build.Modules;

[DependsOn<CodeFormattedNicelyModule>]
[DependsOn<PackProjectsModule>]
public class RunUnitTestsModule : Module<CommandResult[]>
{
    protected override AsyncRetryPolicy<CommandResult[]?> RetryPolicy => CreateRetryPolicy(0);

    /// <inheritdoc/>
    protected override async Task<CommandResult[]?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        return await context.Git().RootDirectory
            .GetFiles(file => file.Path.EndsWith(".csproj", StringComparison.OrdinalIgnoreCase)
                              && file.Path.Contains("UnitTests", StringComparison.OrdinalIgnoreCase))
            .ToAsyncProcessorBuilder()
            .SelectAsync(async unitTestProjectFile => await context.DotNet().Run(new DotNetRunOptions
            {
                Project = unitTestProjectFile.Path,
                NoBuild = true,
                Framework = "net7.0",
                Arguments = ["--coverage", "--coverage-output-format", "cobertura"],
                Configuration = Configuration.Release,
                EnvironmentVariables = new Dictionary<string, string?>
                {
                    ["GITHUB_ACTIONS"] = null,
                },
                Properties = new KeyValue[]
                {
                    new("RunAnalyzersDuringBuild", "false"),
                    new("RunAnalyzers", "false"),
                },
            }, cancellationToken))
            .ProcessInParallel();
    }
}