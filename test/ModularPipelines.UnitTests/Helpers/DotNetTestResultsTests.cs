using ModularPipelines.Context;
using ModularPipelines.DotNet.Extensions;
using ModularPipelines.DotNet.Options;
using ModularPipelines.DotNet.Parsers.NUnitTrx;
using ModularPipelines.Enums;
using ModularPipelines.Exceptions;
using ModularPipelines.Git.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;
using File = ModularPipelines.FileSystem.File;

namespace ModularPipelines.UnitTests.Helpers;

[TUnit.Core.NotInParallel]
public class DotNetTestResultsTests : TestBase
{
    private class DotNetTestWithFailureModule : Module<CommandResult>
    {
        protected override async Task<CommandResult?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            var testProject = context.Git().RootDirectory
                .FindFile(x => x.Name == "ModularPipelines.TestsForTests.csproj")!;

            return await context.DotNet().Test(new DotNetTestOptions
            {
                ProjectSolutionDirectoryDllExe = testProject,
                Framework = "net8.0",
                CommandLogging = CommandLogging.Error,
            }, token: cancellationToken);
        }
    }

    private class DotNetTestWithoutFailureModule : Module<CommandResult>
    {
        public File TrxFile { get; } = File.GetNewTemporaryFilePath();
        
        protected override async Task<CommandResult?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            var testProject = context.Git().RootDirectory
                .FindFile(x => x.Name == "ModularPipelines.TestsForTests.csproj")!;

            return await context.DotNet().Test(new DotNetTestOptions
            {
                ProjectSolutionDirectoryDllExe = testProject,
                Filter = "TestCategory=Pass",
                Framework = "net8.0",
                CommandLogging = CommandLogging.Error,
                Logger = [$"trx;LogFileName={TrxFile}"]
            }, token: cancellationToken);
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
            .Throws.Nothing();
    }

    [Test]
    public async Task Can_Parse_Trx_Manually()
    {
        var module = await RunModule<DotNetTestWithoutFailureModule>();
        var parsedResults = new TrxParser().ParseTrxContents(await module.TrxFile.ReadAsync());

        await Assert.That(parsedResults.UnitTestResults).Has.Count().EqualTo(2);
    }
    
    [Test]
    public async Task Can_Parse_Trx_Using_Helper()
    {
        var module = await RunModule<DotNetTestWithoutFailureModule>();
        
        var parsedResults = await module.Context.Trx().ParseTrxFile(module.TrxFile);

        await Assert.That(parsedResults.UnitTestResults).Has.Count().EqualTo(2);
    }
}