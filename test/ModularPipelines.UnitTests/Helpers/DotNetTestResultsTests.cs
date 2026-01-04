using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Context;
using ModularPipelines.DotNet.Extensions;
using ModularPipelines.DotNet.Options;
using ModularPipelines.DotNet.Parsers.Trx;
using ModularPipelines.Exceptions;
using ModularPipelines.Extensions;
using ModularPipelines.Git.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;
using File = ModularPipelines.FileSystem.File;

namespace ModularPipelines.UnitTests.Helpers;

[TUnit.Core.NotInParallel]
public class DotNetTestResultsTests : TestBase
{
    private class DotNetTestWithFailureModule : Module<CommandResult>
    {
        public override async Task<CommandResult?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            var testProject = context.Git().RootDirectory
                .FindFile(x => x.Name == "ModularPipelines.TestsForTests.csproj")!;

            return await context.DotNet().Test(
                new DotNetTestOptions
                {
                    Framework = "net10.0",
                },
                new CommandExecutionOptions
                {
                    WorkingDirectory = testProject.Folder!.Path,
                    LogSettings = new CommandLoggingOptions
                    {
                        Verbosity = CommandLogVerbosity.Minimal,
                        ShowStandardOutput = false,
                        ShowStandardError = true,
                    },
                },
                cancellationToken);
        }
    }

    private class DotNetTestWithoutFailureModule : Module<CommandResult>
    {
        public static File TrxFile { get; } = File.GetNewTemporaryFilePath();

        public override async Task<CommandResult?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            var testProject = context.Git().RootDirectory
                .FindFile(x => x.Name == "ModularPipelines.TestsForTests.csproj")!;

            return await context.DotNet().Test(
                new DotNetTestOptions
                {
                    Framework = "net10.0",
                    Arguments = 
                    [
                        "--treenode-filter", "/**[Category=Pass]",
                        "--report-trx", "report-trx-filename", TrxFile.Name
                    ],
                },
                new CommandExecutionOptions
                {
                    WorkingDirectory = testProject.Folder!.Path,
                    LogSettings = new CommandLoggingOptions
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
    public async Task Has_Not_Errored()
    {
        await Assert.That(RunModule<DotNetTestWithoutFailureModule>)
            .ThrowsNothing();
    }

    [Test]
    public async Task Can_Parse_Trx_Manually()
    {
        await RunModule<DotNetTestWithoutFailureModule>();
        var parsedResults = new TrxParser().ParseTrxContents(await DotNetTestWithoutFailureModule.TrxFile.ReadAsync());

        await Assert.That(parsedResults.UnitTestResults).HasCount().EqualTo(2);
    }

    [Test]
    public async Task Can_Parse_Trx_Using_Helper()
    {
        var host = await TestPipelineHostBuilder.Create()
            .AddModule<DotNetTestWithoutFailureModule>()
            .BuildHostAsync();

        await host.ExecutePipelineAsync();

        // Get the Trx helper from a module context
        // IPipelineContext is a scoped service, so we need to create a scope
        await using var scope = host.RootServices.CreateAsyncScope();
        var context = scope.ServiceProvider.GetRequiredService<IPipelineContext>();
        var parsedResults = await context.Trx().ParseTrxFile(DotNetTestWithoutFailureModule.TrxFile);

        await Assert.That(parsedResults.UnitTestResults).HasCount().EqualTo(2);
    }
}
