using EnumerableAsyncProcessor.Extensions;
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

namespace ModularPipelines.Build.Modules;

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
        var testFramework = _pipelineSettings.Value.TestFramework;
        var unitTestProjects = context.Git().RootDirectory
            .GetFiles(file => file.Path.EndsWith(".csproj", StringComparison.OrdinalIgnoreCase)
                              && file.Path.Contains("UnitTests", StringComparison.OrdinalIgnoreCase))
            .Select(file => new UnitTestProject(file, testFramework, $"{file.NameWithoutExtension}.trx"))
            .ToList();

        var commandResults = await unitTestProjects
            .ToAsyncProcessorBuilder()
            .SelectAsync(async unitTestProject => await context.DotNet().Run(new DotNetRunOptions
            {
                Project = unitTestProject.ProjectFile.Path,
                NoBuild = true,
                Framework = testFramework,
                Arguments =
                [
                    "--",
                    "--coverage",
                    "--coverage-output-format", "cobertura",
                    "--hangdump",
                    "--hangdump-timeout", "20m",
                    "--report-trx",
                    "--report-trx-filename", unitTestProject.TrxFileName
                ],
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
                OutputLoggingManipulator = RemoveSkippedTestDump,
            },
            cancellationToken))
            .ProcessInParallel();

        await PrintSkippedTestsAsync(context, unitTestProjects, cancellationToken);

        return commandResults;
    }

    private static async Task PrintSkippedTestsAsync(
        IModuleContext context,
        IReadOnlyCollection<UnitTestProject> unitTestProjects,
        CancellationToken cancellationToken)
    {
        var skippedTests = new List<SkippedTest>();

        foreach (var unitTestProject in unitTestProjects)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (!unitTestProject.TrxFile.Exists)
            {
                continue;
            }

            var testResult = await context.Trx().ParseTrxFile(unitTestProject.TrxFile);

            skippedTests.AddRange(testResult.UnitTestResults
                .Where(test => test.Outcome == TestOutcome.NotExecuted)
                .Select(test => new SkippedTest(
                    unitTestProject.ProjectName,
                    test.TestName ?? "-",
                    GetSkippedReason(test))));
        }

        if (skippedTests.Count == 0)
        {
            return;
        }

        var table = new Table
        {
            Border = TableBorder.Rounded,
        };

        table.AddColumn(new TableColumn("[bold]Project[/]").LeftAligned());
        table.AddColumn(new TableColumn("[bold]Skipped Test[/]").LeftAligned());
        table.AddColumn(new TableColumn("[bold]Reason[/]").LeftAligned());

        foreach (var skippedTest in skippedTests
                     .OrderBy(x => x.Project, StringComparer.OrdinalIgnoreCase)
                     .ThenBy(x => x.Test, StringComparer.OrdinalIgnoreCase))
        {
            table.AddRow(
                Markup.Escape(skippedTest.Project),
                Markup.Escape(skippedTest.Test),
                Markup.Escape(skippedTest.Reason));
        }

        ((IConsoleWriter)context.Logger).Write(table);
    }

    private static string GetSkippedReason(UnitTestResult test)
    {
        var reason = test.Output?.DebugTrace
                     ?? test.Output?.ErrorInfo?.Message
                     ?? test.Output?.StdOut;

        if (string.IsNullOrWhiteSpace(reason))
        {
            return "-";
        }

        return reason.Replace("Skipped:", string.Empty, StringComparison.OrdinalIgnoreCase).Trim();
    }

    private static string RemoveSkippedTestDump(string output)
    {
        var filteredLines = new List<string>();
        var skippingSkippedBlock = false;

        foreach (var line in output.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries))
        {
            if (line.StartsWith("skipped ", StringComparison.OrdinalIgnoreCase))
            {
                skippingSkippedBlock = true;
                continue;
            }

            if (skippingSkippedBlock && char.IsWhiteSpace(line[0]))
            {
                continue;
            }

            skippingSkippedBlock = false;
            filteredLines.Add(line);
        }

        return string.Join(Environment.NewLine, filteredLines);
    }

    private sealed record UnitTestProject(File ProjectFile, string Framework, string TrxFileName)
    {
        public string ProjectName => ProjectFile.NameWithoutExtension;

        public File TrxFile => ProjectFile.Folder!
            .GetFolder(Path.Combine("bin", "Release", Framework, "TestResults"))
            .GetFile(TrxFileName);
    }

    private sealed record SkippedTest(string Project, string Test, string Reason);
}
