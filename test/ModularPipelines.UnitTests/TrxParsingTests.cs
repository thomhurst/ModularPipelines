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
using File = ModularPipelines.FileSystem.File;

namespace ModularPipelines.UnitTests;

public class TrxParsingTests : TestBase
{
    public class NUnitModule : IModule<DotNetTestResult>
    {
        public DotNetTestResult? LastResult { get; private set; }

        public async Task<DotNetTestResult?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            var testProject = context.Git().RootDirectory
                .FindFile(x => x.Name == "ModularPipelines.TestsForTests.csproj")!;

            var trxFile = File.GetNewTemporaryFilePath();

            await context.DotNet().Test(new DotNetTestOptions
            {
                ProjectSolutionDirectoryDllExe = testProject,
                Framework = "net10.0",
                CommandLogging = CommandLogging.Error,
                Logger = [$"trx;logfilename={trxFile}"],
                ThrowOnNonZeroExitCode = false
            }, token: cancellationToken);

            var trxContents = await trxFile.ReadAsync(cancellationToken);

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
            .HasCount().EqualTo(1);

        await Assert.That(testResult.UnitTestResults.Where(x => x.Outcome == TestOutcome.NotExecuted))
            .HasCount().EqualTo(1);

        await Assert.That(testResult.UnitTestResults.Where(x => x.Outcome == TestOutcome.Passed))
            .HasCount().EqualTo(2);
    }
}