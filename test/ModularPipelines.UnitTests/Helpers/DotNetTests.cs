using ModularPipelines.Context;
using ModularPipelines.DotNet.Extensions;
using ModularPipelines.DotNet.Options;
using ModularPipelines.Extensions;
using ModularPipelines.Git.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;
using ModularPipelines.TestHelpers.Assertions;

namespace ModularPipelines.UnitTests.Helpers;

public class DotNetTests : TestBase
{
    private class DotNetVersionModule : Module<CommandResult>
    {
        public override async Task<CommandResult?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            // Use main solution explicitly - FindFile returns first match alphabetically
            // which could be ModularPipelines.Analyzers.sln causing flaky failures
            return await context.DotNet().Package.List(new DotNetPackageListOptions
            {
                Project = context.Git().RootDirectory.FindFile(x => x.Name == "ModularPipelines.sln").AssertExists(),
            }, cancellationToken: cancellationToken);
        }
    }

    private class DotNetFormatModule : Module<CommandResult>
    {
        public override async Task<CommandResult?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            return await context.DotNet().Format(new DotNetFormatOptions
            {
                ProjectSolution = context.Git().RootDirectory.FindFile(x => x.Name.Contains("TestsForTests")).AssertExists(),
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
