using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.Build.Modules.UnitTests;
using ModularPipelines.DotNet;
using ModularPipelines.DotNet.Options;
using ModularPipelines.DotNet.Parsers.Trx;
using ModularPipelines.DotNet.Services;
using ModularPipelines.FileSystem;
using ModularPipelines.Options;
using Moq;
using Spectre.Console;

namespace ModularPipelines.Build.UnitTests;

public class RunUnitTestReportingTests
{
    [Test]
    [Arguments("valid")]
    [Arguments("missing")]
    [Arguments("malformed")]
    [Arguments("timeout")]
    [Arguments("noncooperative")]
    public async Task Final_Reporting_Preserves_Execution_Cancellation(string report)
    {
        var directory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(directory);
        using var releaseParser = new ManualResetEventSlim();
        var parserFinished = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        try
        {
            var project = new FilePath(Path.Combine(directory, "Tests.csproj"));
            var trxPath = Path.Combine(directory, "bin", "Release", "net11.0", "TestResults", "test-results.trx");
            using var executionCancellation = new CancellationTokenSource();
            executionCancellation.Cancel();
            var originalCancellation = new OperationCanceledException("Execution canceled", executionCancellation.Token);
            var context = new Mock<IModuleContext> { DefaultValue = DefaultValue.Mock };
            context.Setup(x => x.Logger).Returns(NullLogger.Instance);
            var dotnet = new Mock<IDotNet>();
            dotnet.Setup(x => x.RunAsync(It.IsAny<DotNetRunOptions>(), It.IsAny<CommandExecutionOptions>(), executionCancellation.Token))
                .Returns(() =>
                {
                    // The process has produced its report before final reporting begins.
                    if (report != "missing")
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(trxPath)!);
                        File.WriteAllText(trxPath, report == "malformed" ? "<broken" :
                            File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Data", "test-results.trx")));
                    }

                    return Task.FromException<CommandResult>(originalCancellation);
                });
            context.Setup(x => x.Tools.Get<IDotNet>()).Returns(dotnet.Object);
            var trx = new Mock<ITrx>();
            CancellationToken reportingToken = default;
            trx.Setup(x => x.ParseTrxFileAsync(It.IsAny<FilePath>(), It.IsAny<CancellationToken>()))
                .Returns(async (FilePath file, CancellationToken token) =>
                {
                    reportingToken = token;
                    if (report == "noncooperative")
                    {
                        // Block synchronously, ignoring the reporting token until the caller returns.
                        releaseParser.Wait();
                        parserFinished.SetResult();
                        throw new InvalidOperationException("Late parser failure");
                    }

                    if (report == "timeout")
                    {
                        await Task.Delay(Timeout.InfiniteTimeSpan, token);
                    }

                    return new TrxParser().ParseTrxContents(await file.ReadAsync(token));
                });
            context.Setup(x => x.Tools.Get<ITrx>()).Returns(trx.Object);

            Exception? actual = null;
            try
            {
                await RunUnitTestModule.RunTestProjectAsync(context.Object, project, "net11.0", [], executionCancellation.Token)
                    .WaitAsync(TimeSpan.FromSeconds(20));
            }
            catch (Exception exception)
            {
                actual = exception;
            }

            await Assert.That(ReferenceEquals(actual, originalCancellation)).IsTrue();
            if (report == "missing")
            {
                trx.Verify(x => x.ParseTrxFileAsync(It.IsAny<FilePath>(), It.IsAny<CancellationToken>()), Times.Never);
            }
            else
            {
                await Assert.That(reportingToken.CanBeCanceled).IsTrue();
                await Assert.That(reportingToken == executionCancellation.Token).IsFalse();
            }

            if (report == "valid")
            {
                Mock.Get(context.Object.Console).Verify(x => x.WriteMarkupLine("[red]✗ 1 failed[/]"), Times.Once);
                Mock.Get(context.Object.Console).Verify(x => x.WriteMarkupLine("[dim]⏭ 1 skipped[/]"), Times.Once);
                Mock.Get(context.Object.Console).Verify(x => x.Write(It.IsAny<Table>()), Times.Exactly(2));
            }

            if (report is "timeout" or "noncooperative")
            {
                await Assert.That(reportingToken.IsCancellationRequested).IsTrue();
            }
        }
        finally
        {
            releaseParser.Set();
            if (report == "noncooperative")
            {
                await parserFinished.Task.WaitAsync(TimeSpan.FromSeconds(5));
            }

            Directory.Delete(directory, recursive: true);
        }
    }
}
