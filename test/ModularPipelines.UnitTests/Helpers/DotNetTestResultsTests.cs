using ModularPipelines.Context;
using ModularPipelines.DotNet.Extensions;
using ModularPipelines.DotNet.Options;
using ModularPipelines.Enums;
using ModularPipelines.Exceptions;
using ModularPipelines.Git.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;

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
                Framework = "net7.0",
                CommandLogging = CommandLogging.Error,
            }, token: cancellationToken);
        }
    }

    private class DotNetTestWithoutFailureModule : Module<CommandResult>
    {
        protected override async Task<CommandResult?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            var testProject = context.Git().RootDirectory
                .FindFile(x => x.Name == "ModularPipelines.TestsForTests.csproj")!;

            return await context.DotNet().Test(new DotNetTestOptions
            {
                ProjectSolutionDirectoryDllExe = testProject,
                Filter = "TestCategory=Pass",
                Framework = "net7.0",
                CommandLogging = CommandLogging.Error,
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
}