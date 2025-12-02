using EnumerableAsyncProcessor.Extensions;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.DotNet.Extensions;
using ModularPipelines.DotNet.Options;
using ModularPipelines.Git.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Modules.Behaviors;
using Polly;
using Polly.Retry;

namespace ModularPipelines.Build.Modules;

[DependsOn<CodeFormattedNicelyModule>(IgnoreIfNotRegistered = true)]
public class RunUnitTestsModule : Module<CommandResult[]>, IRetryable<CommandResult[]>
{
    public AsyncRetryPolicy<CommandResult[]?> GetRetryPolicy(IPipelineContext context)
    {
        return Policy<CommandResult[]?>.Handle<Exception>().RetryAsync(0);
    }

    public override async Task<CommandResult[]?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        return await context.Git().RootDirectory
            .GetFiles(file => file.Path.EndsWith(".csproj", StringComparison.OrdinalIgnoreCase)
                              && file.Path.Contains("UnitTests", StringComparison.OrdinalIgnoreCase))
            .ToAsyncProcessorBuilder()
            .SelectAsync(async unitTestProjectFile => await context.DotNet().Run(new DotNetRunOptions
            {
                Project = unitTestProjectFile.Path,
                NoBuild = true,
                Framework = "net10.0",
                Arguments = ["--coverage", "--coverage-output-format", "cobertura"],
                Configuration = Configuration.Release,
                EnvironmentVariables = new Dictionary<string, string?>
                {
                    ["GITHUB_ACTIONS"] = null,
                    ["GITHUB_STEP_SUMMARY"] = null,
                },
                Properties =
                [
                    new("RunAnalyzersDuringBuild", "false"),
                    new("RunAnalyzers", "false")
                ],
            }, cancellationToken))
            .ProcessInParallel();
    }
}
