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
        protected internal override async Task<CommandResult> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            var repositoryInfo = await context.Git().Information.GetInfoAsync()
                ?? throw new InvalidOperationException("Git repository information is unavailable.");

            // Use the main solution explicitly rather than searching the repository.
            return await context.DotNet().Package.ListAsync(new DotNetPackageListOptions
            {
                Project = repositoryInfo.Root.GetFile("ModularPipelines.sln").AssertExists(),
            }, cancellationToken: cancellationToken);
        }
    }

    private class DotNetFormatModule : Module<CommandResult>
    {
        protected internal override async Task<CommandResult> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            var repositoryInfo = await context.Git().Information.GetInfoAsync()
                ?? throw new InvalidOperationException("Git repository information is unavailable.");

            return await context.DotNet().FormatAsync(new DotNetFormatOptions
            {
                ProjectSolution = repositoryInfo.Root.GetFolder("test")
                    .GetFolder("ModularPipelines.TestsForTests")
                    .GetFile("ModularPipelines.TestsForTests.csproj")
                    .AssertExists(),
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
