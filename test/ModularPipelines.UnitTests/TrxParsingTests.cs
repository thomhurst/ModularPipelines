using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Context;
using ModularPipelines.DotNet;
using ModularPipelines.DotNet.Enums;
using ModularPipelines.DotNet.Extensions;
using ModularPipelines.DotNet.Options;
using ModularPipelines.DotNet.Parsers.Trx;
using ModularPipelines.Engine;
using ModularPipelines.Enums;
using ModularPipelines.Extensions;
using ModularPipelines.Git.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests;

public class TrxParsingTests : TestBase
{
#pragma warning disable CS0618 // Type or member is obsolete - testing legacy CommandLogging enum
    public class NUnitModule : Module<DotNetTestResult>
    {
        public DotNetTestResult? LastResult { get; private set; }

        public override async Task<DotNetTestResult?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            var testProject = context.Git().RootDirectory
                .FindFile(x => x.Name == "ModularPipelines.TestsForTests.csproj")!;

            // Create a temp directory for test results
            var resultsDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(resultsDirectory);

            const string trxFileName = "test-results.trx";

            await context.DotNet().Test(new DotNetTestOptions
            {
                Framework = "net10.0",
                CommandLogging = CommandLogging.Error,
                ResultsDirectory = resultsDirectory,
                Logger = [$"trx;logfilename={trxFileName}"],
                ThrowOnNonZeroExitCode = false,
                WorkingDirectory = testProject.Folder!.Path
            }, token: cancellationToken);

            var trxFilePath = Path.Combine(resultsDirectory, trxFileName);
            var trxContents = await System.IO.File.ReadAllTextAsync(trxFilePath, cancellationToken);

            LastResult = new TrxParser().ParseTrxContents(trxContents);
            return LastResult;
        }
    }
#pragma warning restore CS0618

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
            .HasCount().EqualTo(1);

        await Assert.That(testResult.UnitTestResults.Where(x => x.Outcome == TestOutcome.NotExecuted))
            .HasCount().EqualTo(1);

        await Assert.That(testResult.UnitTestResults.Where(x => x.Outcome == TestOutcome.Passed))
            .HasCount().EqualTo(2);
    }
}