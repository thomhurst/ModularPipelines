using ModularPipelines.Context;
using ModularPipelines.DotNet;
using ModularPipelines.DotNet.Enums;
using ModularPipelines.DotNet.Exceptions;
using ModularPipelines.DotNet.Extensions;
using ModularPipelines.DotNet.Options;
using ModularPipelines.Enums;
using ModularPipelines.Exceptions;
using ModularPipelines.Git.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;
using TUnit.Assertions;
using TUnit.Core;

namespace ModularPipelines.UnitTests.Helpers;

[Parallelizable(ParallelScope.None)]
public class DotNetTestResultsTests : TestBase
{
    private class DotNetTestWithFailureModule : Module<DotNetTestResult>
    {
        protected override async Task<DotNetTestResult?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
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

    private class DotNetTestWithoutFailureModule : Module<DotNetTestResult>
    {
        protected override async Task<DotNetTestResult?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
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
    public void Has_Errored()
    {
        var moduleFailedException = Assert.ThrowsAsync<ModuleFailedException>(async () => await RunModule<DotNetTestWithFailureModule>())!;

        var exception = moduleFailedException.InnerException as DotNetTestFailedException;

        var unitTestResults = exception?.DotNetTestResult?.UnitTestResults;

        Assert.Multiple(() =>
        {
            Assert.That(exception, Is.Not.Null().() => $"Exception is null: {exception}");
            Assert.That(exception?.Message, Is.Not.Null().And.Not.Empty()).() => $"Exception message is null: {exception?.Message}");
            Assert.That(unitTestResults, Is.Not.Null().() => $"Unit test results are null: {exception!.CommandResult}");
            Assert.That(exception?.DotNetTestResult, Is.Not.Null().() => $"DotNetTestResult is null: {exception?.DotNetTestResult}");
            Assert.That(exception!.DotNetTestResult!.Successful).Is.False();
            Assert.That(unitTestResults!.Where(x => x.Outcome == TestOutcome.Failed).ToList()).Has.Count.EqualTo(1);
            Assert.That(unitTestResults!.Where(x => x.Outcome == TestOutcome.NotExecuted).ToList()).Has.Count.EqualTo(1);
            Assert.That(unitTestResults!.Where(x => x.Outcome == TestOutcome.Passed).ToList()).Has.Count.EqualTo(2);
        });
    }

    [Test]
    public async Task Has_Not_Errored()
    {
        var module = await RunModule<DotNetTestWithoutFailureModule>();

        var result = await module;

        var unitTestResults = result.Value!.UnitTestResults;

        Assert.Multiple(() =>
        {
            Assert.That(result.ModuleResultType).Is.EqualTo(ModuleResultType.Success);
            Assert.That(unitTestResults?.Where(x => x.Outcome == TestOutcome.Failed).ToList()).Has.Count.EqualTo(0);
            Assert.That(unitTestResults?.Where(x => x.Outcome == TestOutcome.Passed).ToList()).Has.Count.EqualTo(2);
        });
    }
}