using System.IO;
using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Context;
using ModularPipelines.DotNet.Extensions;
using ModularPipelines.DotNet.Options;
using ModularPipelines.DotNet.Parsers.Trx;
using ModularPipelines.Exceptions;
using ModularPipelines.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using ModularPipelines.TestHelpers;
using ModularPipelines.TestHelpers.Assertions;
using ModularPipelines.FileSystem;

namespace ModularPipelines.DotNet.UnitTests;

[TUnit.Core.NotInParallel]
public class DotNetTestResultsTests : TestBase
{
    private static readonly FilePath TrxFixture = new(
        Path.Combine(AppContext.BaseDirectory, "Data", "test-results.trx"));

    private class DotNetTestWithFailureModule : Module<CommandResult>
    {
        protected internal override async Task<CommandResult> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            var testProject = TestProjectPaths.TestsForTestsProject;

            return await context.DotNet().TestAsync(
                new DotNetTestOptions
                {
                    Framework = "net10.0",
                },
                new CommandExecutionOptions
                {
                    WorkingDirectory = testProject.Folder!.Path,
                    Logging = new CommandLoggingOptions
                    {
                        Verbosity = CommandLogVerbosity.Minimal,
                        ShowStandardOutput = false,
                        ShowStandardError = true,
                    },
                },
                cancellationToken);
        }
    }

    [Test]
    public async Task Has_Errored()
    {
        await Assert.ThrowsAsync<ModuleFailedException>(async () => await RunModule<DotNetTestWithFailureModule>());
    }

    [Test]
    public async Task Can_Parse_Trx_Manually()
    {
        var parsedResults = new TrxParser().ParseTrxContents(await TrxFixture.ReadAsync());

        await Assert.That(parsedResults.UnitTestResults).Count().IsEqualTo(4);
    }

    [Test]
    public async Task Can_Parse_Trx_Using_Helper()
    {
        var host = await TestPipelineBuilder.Create()
            .AddModule<TrueModule>()
            .BuildAsync();

        // Get the Trx helper from a pipeline context
        // IPipelineContext is a scoped service, so we need to create a scope
        await using var scope = host.Services.CreateAsyncScope();
        var context = scope.ServiceProvider.GetRequiredService<IPipelineContext>();
        var parsedResults = await context.Trx().ParseTrxFile(TrxFixture);

        await Assert.That(parsedResults.UnitTestResults).Count().IsEqualTo(4);
    }
}
