using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Attributes;
using ModularPipelines;
using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Extensions;
using ModularPipelines.GitHub;
using ModularPipelines.GitHub.Attributes;
using ModularPipelines.TestHelpers;
using Moq;
using ModularPipelines.Enums;

namespace ModularPipelines.GitHub.UnitTests.Execution;

public class SkipDependabotAttributeTests : TestBase
{
    private class CanRunCondition : IRunCondition
    {
        public Task<bool> EvaluateAsync(IPipelineContext pipelineContext)
        {
            return Task.FromResult(true);
        }
    }

    private class CannotRunCondition : IRunCondition
    {
        public Task<bool> EvaluateAsync(IPipelineContext pipelineContext)
        {
            return Task.FromResult(false);
        }
    }

    [SkipIfDependabot]
    private class Module1 : SimpleTestModule<bool>
    {
        protected override bool Result => true;
    }

    [SkipIfDependabot]
    [RunIfAny<CanRunCondition>]
    private class Module2 : SimpleTestModule<bool>
    {
        protected override bool Result => true;
    }

    [SkipIfDependabot]
    [RunIfAny<CannotRunCondition>]
    private class Module3 : SimpleTestModule<bool>
    {
        protected override bool Result => true;
    }

    [SkipIfDependabot]
    [RunIfAny<CanRunCondition, CannotRunCondition>]
    private class Module4 : SimpleTestModule<bool>
    {
        protected override bool Result => true;
    }

    [Test]
    public async Task Will_Not_Skip_If_Not_Dependabot()
    {
        var environmentVariables = new Mock<IGitHubEnvironmentVariables>();

        var host = await TestPipelineBuilder.Create()
            .ConfigureServices(collection => collection.AddSingleton(environmentVariables.Object))
            .AddModule<Module1>()
            .BuildAsync();

        await host.RunAsync();

        var resultRegistry = host.Services.GetRequiredService<IModuleResultRegistry>();
        var moduleResult = resultRegistry.GetResult(typeof(Module1))!;
        await Assert.That(moduleResult.Status).IsEqualTo(ModuleStatus.Succeeded);
    }

    [Test]
    public async Task Will_Skip_If_Dependabot()
    {
        var environmentVariables = new Mock<IGitHubEnvironmentVariables>();

        environmentVariables.Setup(x => x.Actor)
            .Returns("dependabot[bot]");

        var host = await TestPipelineBuilder.Create()
            .ConfigureServices(collection => collection.AddSingleton(environmentVariables.Object))
            .AddModule<Module1>()
            .BuildAsync();

        await host.RunAsync();

        var resultRegistry = host.Services.GetRequiredService<IModuleResultRegistry>();
        var moduleResult = resultRegistry.GetResult(typeof(Module1))!;
        await Assert.That(moduleResult.Status).IsEqualTo(ModuleStatus.Skipped);
    }

    [Test]
    public async Task Will_Run_When_Combination_Of_Mandatory_And_Runnable_Run_Category()
    {
        var environmentVariables = new Mock<IGitHubEnvironmentVariables>();

        var host = await TestPipelineBuilder.Create()
            .ConfigureServices(collection => collection.AddSingleton(environmentVariables.Object))
            .AddModule<Module2>()
            .BuildAsync();

        await host.RunAsync();

        var resultRegistry = host.Services.GetRequiredService<IModuleResultRegistry>();
        var moduleResult = resultRegistry.GetResult(typeof(Module2))!;
        await Assert.That(moduleResult.Status).IsEqualTo(ModuleStatus.Succeeded);
    }

    [Test]
    public async Task Will__Not_Run_When_Combination_Of_Mandatory_And_Non_Runnable_Run_Category()
    {
        var environmentVariables = new Mock<IGitHubEnvironmentVariables>();

        var host = await TestPipelineBuilder.Create()
            .ConfigureServices(collection => collection.AddSingleton(environmentVariables.Object))
            .AddModule<Module3>()
            .BuildAsync();

        await host.RunAsync();

        var resultRegistry = host.Services.GetRequiredService<IModuleResultRegistry>();
        var moduleResult = resultRegistry.GetResult(typeof(Module3))!;
        await Assert.That(moduleResult.Status).IsEqualTo(ModuleStatus.Skipped);
    }

    [Test]
    public async Task Will_Run_When_Combination_Of_Mandatory_And_Runnable_Run_Category2()
    {
        var environmentVariables = new Mock<IGitHubEnvironmentVariables>();

        var host = await TestPipelineBuilder.Create()
            .ConfigureServices(collection => collection.AddSingleton(environmentVariables.Object))
            .AddModule<Module4>()
            .BuildAsync();

        await host.RunAsync();

        var resultRegistry = host.Services.GetRequiredService<IModuleResultRegistry>();
        var moduleResult = resultRegistry.GetResult(typeof(Module4))!;
        await Assert.That(moduleResult.Status).IsEqualTo(ModuleStatus.Succeeded);
    }
}
