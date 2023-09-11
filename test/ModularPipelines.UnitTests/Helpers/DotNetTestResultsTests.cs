using ModularPipelines.Context;
using ModularPipelines.DotNet;
using ModularPipelines.DotNet.Enums;
using ModularPipelines.DotNet.Exceptions;
using ModularPipelines.DotNet.Extensions;
using ModularPipelines.DotNet.Options;
using ModularPipelines.Exceptions;
using ModularPipelines.Git.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.UnitTests.Helpers;

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
                TargetPath = testProject,
                LogInput = true,
                LogOutput = true
            }, cancellationToken: cancellationToken);
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
                TargetPath = testProject,
                Filter = "TestCategory=Pass",
                LogInput = true,
                LogOutput = true
            }, cancellationToken: cancellationToken);
        }
    }

    [Test, Retry(3)]
    public void Has_Errored()
    {
        var moduleFailedException = Assert.ThrowsAsync<ModuleFailedException>(async () => await RunModule<DotNetTestWithFailureModule>())!;

        var exception = moduleFailedException.InnerException as DotNetTestFailedException;
        
        var unitTestResults = exception?.DotNetTestResult?.UnitTestResults;
        
        Assert.Multiple(() =>
        {
            Assert.That(exception?.DotNetTestResult?.Successful, Is.False);
            Assert.That(unitTestResults?.Where(x => x.Outcome == TestOutcome.Failed).ToList(), Has.Count.EqualTo(1));
            Assert.That(unitTestResults?.Where(x => x.Outcome == TestOutcome.NotExecuted).ToList(), Has.Count.EqualTo(1));
            Assert.That(unitTestResults?.Where(x => x.Outcome == TestOutcome.Passed).ToList(), Has.Count.EqualTo(2));
        });
    }
    
    [Test, Retry(3)]
    public async Task Has_Not_Errored()
    {
        var module = await RunModule<DotNetTestWithoutFailureModule>();

        var result = await module;

        var unitTestResults = result.Value!.UnitTestResults;
        
        Assert.Multiple(() =>
        {
            Assert.That(result.ModuleResultType, Is.EqualTo(ModuleResultType.Success));
            Assert.That(unitTestResults?.Where(x => x.Outcome == TestOutcome.Failed).ToList(), Has.Count.EqualTo(0));
            Assert.That(unitTestResults?.Where(x => x.Outcome == TestOutcome.Passed).ToList(), Has.Count.EqualTo(2));
        });
    }
}
