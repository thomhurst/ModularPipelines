using EnumerableAsyncProcessor.Extensions;
using Microsoft.Extensions.Logging;
using ModularPipelines.Attributes;
using ModularPipelines.Build.Models;
using ModularPipelines.Context;
using ModularPipelines.DotNet.Extensions;
using ModularPipelines.DotNet.Options;
using ModularPipelines.Git.Extensions;
using ModularPipelines.Modules;
using Polly.Retry;
using File = ModularPipelines.FileSystem.File;

namespace ModularPipelines.Build.Modules.Tests;

/// <summary>
/// Abstract base module for running unit tests on a specific operating system.
/// Concrete implementations specify the OS requirement using [RequiresOs] attribute.
/// </summary>
[DependsOn<CodeFormattedNicelyModule>]
public abstract class RunUnitTestsModule : Module<TestExecutionResult>
{
    protected override AsyncRetryPolicy<TestExecutionResult?> RetryPolicy => CreateRetryPolicy(0);

    /// <inheritdoc/>
    protected override async Task<TestExecutionResult?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        var testResults = await context.Git().RootDirectory
            .GetFiles(file => file.Path.EndsWith(".csproj", StringComparison.OrdinalIgnoreCase)
                              && file.Path.Contains("UnitTests", StringComparison.OrdinalIgnoreCase))
            .ToAsyncProcessorBuilder()
            .SelectAsync(async unitTestProjectFile => await context.DotNet().Run(new DotNetRunOptions
            {
                Project = unitTestProjectFile.Path,
                NoBuild = true,
                Framework = "net9.0",
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

        // Find all coverage files that were generated
        var coverageFiles = context.Git().RootDirectory
            .GetFiles(file => file.Name.Contains("cobertura", StringComparison.OrdinalIgnoreCase)
                              && file.Extension == ".xml")
            .ToList();

        Context.Logger.LogInformation(
            "Test execution completed. Found {CoverageFileCount} coverage files",
            coverageFiles.Count);

        return new TestExecutionResult
        {
            TestResults = testResults.ToArray(),
            CoverageFiles = coverageFiles,
        };
    }
}
