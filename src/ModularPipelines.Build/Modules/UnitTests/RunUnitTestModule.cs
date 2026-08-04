using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines;
using ModularPipelines.Attributes;
using ModularPipelines.Build.Settings;
using ModularPipelines.Configuration;
using ModularPipelines.Context;
using ModularPipelines.DotNet.Enums;
using ModularPipelines.DotNet.Extensions;
using ModularPipelines.DotNet.Options;
using ModularPipelines.DotNet.Parsers.Trx;
using ModularPipelines.Git.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using Polly;
using Spectre.Console;
using File = ModularPipelines.FileSystem.File;

namespace ModularPipelines.Build.Modules.UnitTests;

/// <summary>
/// Runs a unit test project and renders its skipped tests as structured output.
/// </summary>
[DependsOn<BuildSolutionsModule>]
[ConsumesArtifact(typeof(BuildSolutionsModule), "build-output", RestorePath = "../../")]
[RunIfAll<ModularPipelines.Conditions.OnLinux>]
[RequiresCapability("linux")]
public abstract partial class RunUnitTestModule(IOptions<PipelineSettings> pipelineSettings) : Module<CommandResult>
{
    private const string TrxFileName = "test-results.trx";

    protected abstract string TestProjectFileName { get; }

    protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
        .WithSkipWhen(GetSkipDecision)
        .Advanced
        .WithRetryPolicy(Policy.Handle<Exception>().RetryAsync(0))
        .Build();

    protected virtual SkipDecision GetSkipDecision(IModuleContext context) => SkipDecision.DoNotSkip;

    protected override async Task<CommandResult> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        var repositoryInfo = await context.Git().Information.GetInfoAsync().ConfigureAwait(false)
            ?? throw new InvalidOperationException("Git repository information is unavailable.");
        var testProject = repositoryInfo.Root
            .GetFiles(file => file.Name.Equals(TestProjectFileName, StringComparison.OrdinalIgnoreCase))
            .Single();
        var trxFile = GetTrxFile(testProject);

        if (trxFile.Exists)
        {
            await trxFile.DeleteAsync(cancellationToken);
        }

        try
        {
            return await context.DotNet().RunAsync(new DotNetRunOptions
            {
                Project = testProject.Path,
                NoBuild = true,
                Framework = pipelineSettings.Value.TestFramework,
                Arguments =
                [
                    "--coverage",
                    "--coverage-output-format", "cobertura",
                    "--hangdump",
                    "--hangdump-timeout", "20m",
                    "--results-directory", trxFile.Folder!.Path,
                    "--report-trx",
                    "--report-trx-filename", TrxFileName,
                ],
                Configuration = "Release",
                Properties =
                [
                    new("RunAnalyzersDuringBuild", "false"),
                    new("RunAnalyzers", "false"),
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
                OutputLoggingManipulator = RemoveSkippedTestOutput,
            },
            cancellationToken);
        }
        finally
        {
            await PrintSkippedTests(context, trxFile);
        }
    }

    private File GetTrxFile(File testProject)
    {
        return testProject.Folder!
            .GetFolder($"bin/Release/{pipelineSettings.Value.TestFramework}/TestResults")
            .GetFile(TrxFileName);
    }

    private static async Task PrintSkippedTests(IModuleContext context, File trxFile)
    {
        try
        {
            await PrintSkippedTestsCore(context, trxFile);
        }
        catch (Exception exception)
        {
            context.Logger.LogWarning(
                exception,
                "Unable to render skipped test results from {TrxFile}",
                trxFile.Path);
        }
    }

    private static async Task PrintSkippedTestsCore(IModuleContext context, File trxFile)
    {
        if (!trxFile.Exists)
        {
            return;
        }

        var testResults = await context.Trx().ParseTrxFile(trxFile);
        var skippedTests = testResults.UnitTestResults
            .Where(result => result.Outcome == TestOutcome.NotExecuted)
            .ToList();

        if (skippedTests.Count == 0)
        {
            return;
        }

        var table = new Table
        {
            Border = TableBorder.Rounded,
        };

        table.AddColumn(new TableColumn("[bold]Test[/]").LeftAligned());
        table.AddColumn(new TableColumn("[bold]Reason[/]").LeftAligned());

        foreach (var skippedTest in skippedTests)
        {
            table.AddRow(
                Markup.Escape(skippedTest.TestName ?? string.Empty),
                Markup.Escape(GetSkipReason(skippedTest)));
        }

        var consoleWriter = context.GetService<IConsoleWriter>();
        consoleWriter.LogToConsole($"[dim]⏭ {skippedTests.Count} skipped[/]");
        consoleWriter.Write(table);
    }

    private static string GetSkipReason(UnitTestResult testResult)
    {
        const string skippedPrefix = "Skipped:";
        var debugTrace = testResult.Output?.DebugTrace?.Trim();

        return debugTrace?.StartsWith(skippedPrefix, StringComparison.OrdinalIgnoreCase) == true
            ? debugTrace[skippedPrefix.Length..].Trim()
            : debugTrace ?? string.Empty;
    }

    private static string RemoveSkippedTestOutput(string output)
    {
        return SkippedTestOutputRegex().Replace(output, string.Empty).TrimStart();
    }

    [GeneratedRegex(
        @"(?m)^skipped .+ \([^)]+\)\r?\n(?:  .*(?:\r?\n|$))?\r?\n?")]
    private static partial Regex SkippedTestOutputRegex();
}
