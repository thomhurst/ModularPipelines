using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.GitHub;
using ModularPipelines.GitHub.Attributes;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;
using Moq;
using Status = ModularPipelines.Enums.Status;

namespace ModularPipelines.UnitTests;

public class SkipDependabotAttributeTests : TestBase
{
    private class CanRunAttribute : RunConditionAttribute
    {
        public override Task<bool> Condition(IPipelineHookContext pipelineContext)
        {
            return Task.FromResult(true);
        }
    }

    private class CannotRunAttribute : RunConditionAttribute
    {
        public override Task<bool> Condition(IPipelineHookContext pipelineContext)
        {
            return Task.FromResult(false);
        }
    }

    [SkipIfDependabot]
    private class Module1 : Module
    {
        protected override Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            return NothingAsync();
        }
    }

    [SkipIfDependabot]
    [CanRun]
    private class Module2 : Module
    {
        protected override Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            return NothingAsync();
        }
    }

    [SkipIfDependabot]
    [CannotRun]
    private class Module3 : Module
    {
        protected override Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            return NothingAsync();
        }
    }

    [SkipIfDependabot]
    [CanRun]
    [CannotRun]
    private class Module4 : Module
    {
        protected override Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            return NothingAsync();
        }
    }

    [Test]
    public async Task Will_Not_Skip_If_Not_Dependabot()
    {
        var environmentVariables = new Mock<IGitHubEnvironmentVariables>();

        var pipelineSummary = await TestPipelineHostBuilder.Create()
            .ConfigureServices((_, collection) => collection.AddSingleton(environmentVariables.Object))
            .AddModule<Module1>()
            .ExecutePipelineAsync();
        await Assert.That(pipelineSummary.Modules.First().Status).IsEqualTo(Status.Successful);
    }

    [Test]
    public async Task Will_Skip_If_Dependabot()
    {
        var environmentVariables = new Mock<IGitHubEnvironmentVariables>();

        environmentVariables.Setup(x => x.Actor)
            .Returns("dependabot[bot]");

        var pipelineSummary = await TestPipelineHostBuilder.Create()
            .ConfigureServices((_, collection) => collection.AddSingleton(environmentVariables.Object))
            .AddModule<Module1>()
            .ExecutePipelineAsync();
        await Assert.That(pipelineSummary.Modules.First().Status).IsEqualTo(Status.Skipped);
    }

    [Test]
    public async Task Will_Run_When_Combination_Of_Mandatory_And_Runnable_Run_Category()
    {
        var environmentVariables = new Mock<IGitHubEnvironmentVariables>();

        var pipelineSummary = await TestPipelineHostBuilder.Create()
            .ConfigureServices((_, collection) => collection.AddSingleton(environmentVariables.Object))
            .AddModule<Module2>()
            .ExecutePipelineAsync();
        await Assert.That(pipelineSummary.Modules.First().Status).IsEqualTo(Status.Successful);
    }

    [Test]
    public async Task Will__Not_Run_When_Combination_Of_Mandatory_And_Non_Runnable_Run_Category()
    {
        var environmentVariables = new Mock<IGitHubEnvironmentVariables>();

        var pipelineSummary = await TestPipelineHostBuilder.Create()
            .ConfigureServices((_, collection) => collection.AddSingleton(environmentVariables.Object))
            .AddModule<Module3>()
            .ExecutePipelineAsync();
        await Assert.That(pipelineSummary.Modules.First().Status).IsEqualTo(Status.Skipped);
    }

    [Test]
    public async Task Will_Run_When_Combination_Of_Mandatory_And_Runnable_Run_Category2()
    {
        var environmentVariables = new Mock<IGitHubEnvironmentVariables>();

        var pipelineSummary = await TestPipelineHostBuilder.Create()
            .ConfigureServices((_, collection) => collection.AddSingleton(environmentVariables.Object))
            .AddModule<Module4>()
            .ExecutePipelineAsync();
        await Assert.That(pipelineSummary.Modules.First().Status).IsEqualTo(Status.Successful);
    }
}