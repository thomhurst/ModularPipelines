using ModularPipelines.Context;
using ModularPipelines.DotNet.Options;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;
using ModularPipelines.TestHelpers.Assertions;

namespace ModularPipelines.DotNet.UnitTests;

public class DotNetTests : TestBase
{
    private class DotNetVersionModule : Module<CommandResult>
    {
        protected internal override async Task<CommandResult> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            // Use the main solution explicitly; the repository contains several solutions.
            return await context.Tools.DotNet.Package.ListAsync(new DotNetPackageListOptions
            {
                Project = TestProjectPaths.CoreSolution,
            }, cancellationToken: cancellationToken);
        }
    }

    private class DotNetFormatModule : Module<CommandResult>
    {
        protected internal override async Task<CommandResult> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            return await context.Tools.DotNet.FormatAsync(new DotNetFormatOptions
            {
                ProjectSolution = TestProjectPaths.TestsForTestsProject,
            }, cancellationToken: cancellationToken);
        }
    }

    [Test]
    [Skip("Flaky on CI - dotnet list package on full solution times out")]
    public async Task Has_Not_Errored()
    {
        var moduleResult = await await RunModule<DotNetVersionModule>();

        await ModuleResultAssertions.AssertSuccessWithValue(moduleResult);
    }

    [Test]
    [Skip("Temporarily disabled")]
    public async Task Format_Has_Not_Errored()
    {
        var moduleResult = await await RunModule<DotNetFormatModule>();

        await ModuleResultAssertions.AssertSuccessWithValue(moduleResult);
    }
}
