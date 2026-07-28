using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Context;
using ModularPipelines.DotNet;
using ModularPipelines.DotNet.Enums;
using ModularPipelines.DotNet.Extensions;
using ModularPipelines.DotNet.Options;
using ModularPipelines.DotNet.Parsers.Trx;
using ModularPipelines.Engine;
using ModularPipelines.Extensions;
using ModularPipelines.Git.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests.Models;

public class TrxParsingTests : TestBase
{
    [Test]
    public async Task ParsesSkippedTestReason()
    {
        const string trx = """
                           <TestRun>
                             <Results>
                               <UnitTestResult executionId="execution" testId="test" testName="Skipped_Test"
                                 computerName="agent" duration="00:00:00" startTime="2026-07-27T12:00:00Z"
                                 endTime="2026-07-27T12:00:00Z" testType="type" outcome="NotExecuted"
                                 testListId="list" relativeResultsDirectory="results">
                                 <Output>
                                   <DebugTrace>Skipped: Linux only test</DebugTrace>
                                 </Output>
                               </UnitTestResult>
                             </Results>
                             <ResultSummary outcome="Completed">
                               <Counters total="1" executed="0" passed="0" failed="0" error="0"
                                 timeout="0" aborted="0" inconclusive="0" passedButRunAborted="0"
                                 notRunnable="0" notExecuted="1" disconnected="0" warning="0"
                                 completed="0" inProgress="0" pending="0" />
                             </ResultSummary>
                           </TestRun>
                           """;

        var result = new TrxParser().ParseTrxContents(trx);

        await Assert.That(result.UnitTestResults.Single().Output?.DebugTrace)
            .IsEqualTo("Skipped: Linux only test");
    }

    public class NUnitModule : Module<DotNetTestResult>
    {
        public DotNetTestResult? LastResult { get; private set; }

        protected internal override async Task<DotNetTestResult?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            var repositoryInfo = await context.Git().Information.GetInfoAsync()
                ?? throw new InvalidOperationException("Git repository information is unavailable.");
            var testProject = repositoryInfo.Root
                .FindFile(x => x.Name == "ModularPipelines.TestsForTests.csproj")!;

            // Create a temp directory for test results
            var resultsDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(resultsDirectory);

            const string trxFileName = "test-results.trx";

            await context.DotNet().Test(
                new DotNetTestOptions
                {
                    Framework = "net10.0",
                    ResultsDirectory = resultsDirectory,
                    Arguments = ["--report-trx", "--report-trx-filename", trxFileName]
                },
                new CommandExecutionOptions
                {
                    WorkingDirectory = testProject.Folder!.Path,
                    ThrowOnNonZeroExitCode = false,
                    LogSettings = new CommandLoggingOptions
                    {
                        Verbosity = CommandLogVerbosity.Minimal,
                        ShowStandardOutput = false,
                        ShowStandardError = true,
                    },
                },
                cancellationToken);

            var trxFilePath = Path.Combine(resultsDirectory, trxFileName);
            var trxContents = await System.IO.File.ReadAllTextAsync(trxFilePath, cancellationToken);

            LastResult = new TrxParser().ParseTrxContents(trxContents);
            return LastResult;
        }
    }

    [Test]
    public async Task NUnit()
    {
        var host = await TestPipelineHostBuilder.Create()
            .AddModule<NUnitModule>()
            .BuildHostAsync();

        await host.ExecutePipelineAsync();

        var module = host.RootServices.GetServices<IModule>().OfType<NUnitModule>().First();
        var testResult = module.LastResult!;

        await Assert.That(testResult.Successful).IsFalse();

        await Assert.That(testResult.UnitTestResults.Where(x => x.Outcome == TestOutcome.Failed))
            .Count().IsEqualTo(1);

        await Assert.That(testResult.UnitTestResults.Where(x => x.Outcome == TestOutcome.NotExecuted))
            .Count().IsEqualTo(1);

        await Assert.That(testResult.UnitTestResults.Where(x => x.Outcome == TestOutcome.Passed))
            .Count().IsEqualTo(2);
    }
}
