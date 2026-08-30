using ModularPipelines.Logging;
using System.Text.RegularExpressions;
using Kevlar;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines;
using ModularPipelines.Attributes;
using ModularPipelines.Build.Settings;
using ModularPipelines.Configuration;
using ModularPipelines.Context;
using ModularPipelines.DotNet.Enums;
using ModularPipelines.DotNet.Options;
using ModularPipelines.DotNet.Parsers.Trx;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using Spectre.Console;
using ModularPipelines.FileSystem;

namespace ModularPipelines.Build.Modules.UnitTests;

/// <summary>
/// Runs a unit test project and renders its failures and skipped tests as structured output.
/// </summary>
[DependsOn<BuildSolutionsModule>]
[ConsumesArtifact(typeof(BuildSolutionsModule), "build-output", RestorePath = "../../")]
[RunIf<ModularPipelines.OnLinux>]
[RequiresCapability("linux")]
public abstract partial class RunUnitTestModule(IOptions<PipelineSettings> pipelineSettings) : Module<CommandResult>
{
    private const int MaximumFailuresToDisplay = 10;
    private const int MaximumFailureMessageLength = 500;
    private const int MaximumFailureMessageLines = 3;
    private const string HangDumpFileName = "hangdump-{pname}-{pid}-{time}.dmp";
    private const string TrxFileName = "test-results.trx";

    protected abstract string TestProjectFileName { get; }

    protected override void Configure(ModuleConfigurationBuilder module) => module
        .WithSkipWhen(GetSkipDecision)
        .WithShield(Shield.Retry(0));

    protected virtual SkipDecision GetSkipDecision(IModuleContext context) => SkipDecision.DoNotSkip;

    protected override async Task<CommandResult> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        var repositoryInfo = await context.Tools.Git.Information.GetInfoAsync().ConfigureAwait(false)
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
            return await context.Tools.DotNet.RunAsync(new DotNetRunOptions
            {
                Project = testProject.Path,
                NoBuild = true,
                Framework = pipelineSettings.Value.TestFramework,
                Arguments =
                [
                    "--coverage",
                    "--coverage-output-format", "cobertura",
                    "--hangdump",
                    "--hangdump-filename", HangDumpFileName,
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
            await PrintTestResults(context, trxFile);
        }
    }

    private FilePath GetTrxFile(FilePath testProject)
    {
        return testProject.Folder!
            .GetFolder($"bin/Release/{pipelineSettings.Value.TestFramework}/TestResults")
            .GetFile(TrxFileName);
    }

    private static async Task PrintTestResults(IModuleContext context, FilePath trxFile)
    {
        try
        {
            await PrintTestResultsCore(context, trxFile);
        }
        catch (Exception exception)
        {
            context.Logger.LogWarning(
                exception,
                "Unable to render test results from {TrxFile}",
                trxFile.Path);
        }
    }

    private static async Task PrintTestResultsCore(IModuleContext context, FilePath trxFile)
    {
        if (!trxFile.Exists)
        {
            return;
        }

        var testResults = await context.Tools.Trx.ParseTrxFile(trxFile);
        var consoleWriter = context.Services.GetRequiredService<IConsoleWriter>();
        PrintFailedTests(consoleWriter, testResults.UnitTestResults, trxFile.Path);
        PrintSkippedTests(consoleWriter, testResults.UnitTestResults);
    }

    private static void PrintFailedTests(
        IConsoleWriter consoleWriter,
        IReadOnlyCollection<UnitTestResult> testResults,
        string trxFilePath)
    {
        var failedTests = testResults
            .Where(result => result.Outcome is not TestOutcome.Passed
                and not TestOutcome.Completed
                and not TestOutcome.NotExecuted)
            .ToList();

        if (failedTests.Count == 0)
        {
            return;
        }

        var table = new Table
        {
            Border = TableBorder.Rounded,
        };

        table.AddColumn(new TableColumn("[bold]Test[/]").LeftAligned());
        table.AddColumn(new TableColumn("[bold]Outcome[/]").LeftAligned());
        table.AddColumn(new TableColumn("[bold]Message[/]").LeftAligned());
        table.AddColumn(new TableColumn("[bold]Location[/]").LeftAligned());

        foreach (var failedTest in failedTests.Take(MaximumFailuresToDisplay))
        {
            table.AddRow(
                Markup.Escape(failedTest.TestName ?? string.Empty),
                Markup.Escape(failedTest.Outcome?.ToString() ?? TestOutcome.Unknown.ToString()),
                Markup.Escape(GetFailureMessage(failedTest)),
                Markup.Escape(GetFailureLocation(failedTest)));
        }

        consoleWriter.LogToConsole($"[red]✗ {failedTests.Count} failed[/]");
        consoleWriter.Write(table);

        if (failedTests.Count > MaximumFailuresToDisplay)
        {
            consoleWriter.LogToConsole(
                $"[dim]Showing first {MaximumFailuresToDisplay}; "
                + $"{failedTests.Count - MaximumFailuresToDisplay} more in {Markup.Escape(trxFilePath)}[/]");
        }
    }

    private static void PrintSkippedTests(IConsoleWriter consoleWriter, IReadOnlyCollection<UnitTestResult> testResults)
    {
        var skippedTests = testResults
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

        consoleWriter.LogToConsole($"[dim]⏭ {skippedTests.Count} skipped[/]");
        consoleWriter.Write(table);
    }

    private static string GetFailureMessage(UnitTestResult testResult)
    {
        var failureText = testResult.Output?.ErrorInfo?.Message
            ?? testResult.Output?.DebugTrace
            ?? testResult.Output?.StdOut;

        return SummarizeText(failureText, "No failure message recorded.");
    }

    private static string GetFailureLocation(UnitTestResult testResult)
    {
        return GetNonEmptyLines(testResult.Output?.ErrorInfo?.StackTrace)
            .FirstOrDefault()
            ?? "No stack trace recorded.";
    }

    private static string SummarizeText(string? value, string fallback)
    {
        var summary = string.Join(
            Environment.NewLine,
            GetNonEmptyLines(value).Take(MaximumFailureMessageLines));

        if (string.IsNullOrEmpty(summary))
        {
            return fallback;
        }

        return summary.Length <= MaximumFailureMessageLength
            ? summary
            : $"{summary[..(MaximumFailureMessageLength - 3)]}...";
    }

    private static IEnumerable<string> GetNonEmptyLines(string? value)
    {
        return (value ?? string.Empty)
            .Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
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
