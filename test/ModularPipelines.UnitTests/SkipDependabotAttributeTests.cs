using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Extensions;
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
    private class Module1 : Module<IDictionary<string, object>?>
    {
        public override async Task<IDictionary<string, object>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return null;
        }
    }

    [SkipIfDependabot]
    [CanRun]
    private class Module2 : Module<IDictionary<string, object>?>
    {
        public override async Task<IDictionary<string, object>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return null;
        }
    }

    [SkipIfDependabot]
    [CannotRun]
    private class Module3 : Module<IDictionary<string, object>?>
    {
        public override async Task<IDictionary<string, object>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return null;
        }
    }

    [SkipIfDependabot]
    [CanRun]
    [CannotRun]
    private class Module4 : Module<IDictionary<string, object>?>
    {
        public override async Task<IDictionary<string, object>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return null;
        }
    }

    [Test]
    public async Task Will_Not_Skip_If_Not_Dependabot()
    {
        var environmentVariables = new Mock<IGitHubEnvironmentVariables>();

        var host = await TestPipelineHostBuilder.Create()
            .ConfigureServices((_, collection) => collection.AddSingleton(environmentVariables.Object))
            .AddModule<Module1>()
            .BuildHostAsync();

        await host.ExecutePipelineAsync();

        var resultRegistry = host.RootServices.GetRequiredService<IModuleResultRegistry>();
        var moduleResult = resultRegistry.GetResult(typeof(Module1))!;
        await Assert.That(moduleResult.ModuleStatus).IsEqualTo(Status.Successful);
    }

    [Test]
    public async Task Will_Skip_If_Dependabot()
    {
        var environmentVariables = new Mock<IGitHubEnvironmentVariables>();

        environmentVariables.Setup(x => x.Actor)
            .Returns("dependabot[bot]");

        var host = await TestPipelineHostBuilder.Create()
            .ConfigureServices((_, collection) => collection.AddSingleton(environmentVariables.Object))
            .AddModule<Module1>()
            .BuildHostAsync();

        await host.ExecutePipelineAsync();

        var resultRegistry = host.RootServices.GetRequiredService<IModuleResultRegistry>();
        var moduleResult = resultRegistry.GetResult(typeof(Module1))!;
        await Assert.That(moduleResult.ModuleStatus).IsEqualTo(Status.Skipped);
    }

    [Test]
    public async Task Will_Run_When_Combination_Of_Mandatory_And_Runnable_Run_Category()
    {
        var environmentVariables = new Mock<IGitHubEnvironmentVariables>();

        var host = await TestPipelineHostBuilder.Create()
            .ConfigureServices((_, collection) => collection.AddSingleton(environmentVariables.Object))
            .AddModule<Module2>()
            .BuildHostAsync();

        await host.ExecutePipelineAsync();

        var resultRegistry = host.RootServices.GetRequiredService<IModuleResultRegistry>();
        var moduleResult = resultRegistry.GetResult(typeof(Module2))!;
        await Assert.That(moduleResult.ModuleStatus).IsEqualTo(Status.Successful);
    }

    [Test]
    public async Task Will__Not_Run_When_Combination_Of_Mandatory_And_Non_Runnable_Run_Category()
    {
        var environmentVariables = new Mock<IGitHubEnvironmentVariables>();

        var host = await TestPipelineHostBuilder.Create()
            .ConfigureServices((_, collection) => collection.AddSingleton(environmentVariables.Object))
            .AddModule<Module3>()
            .BuildHostAsync();

        await host.ExecutePipelineAsync();

        var resultRegistry = host.RootServices.GetRequiredService<IModuleResultRegistry>();
        var moduleResult = resultRegistry.GetResult(typeof(Module3))!;
        await Assert.That(moduleResult.ModuleStatus).IsEqualTo(Status.Skipped);
    }

    [Test]
    public async Task Will_Run_When_Combination_Of_Mandatory_And_Runnable_Run_Category2()
    {
        var environmentVariables = new Mock<IGitHubEnvironmentVariables>();

        var host = await TestPipelineHostBuilder.Create()
            .ConfigureServices((_, collection) => collection.AddSingleton(environmentVariables.Object))
            .AddModule<Module4>()
            .BuildHostAsync();

        await host.ExecutePipelineAsync();

        var resultRegistry = host.RootServices.GetRequiredService<IModuleResultRegistry>();
        var moduleResult = resultRegistry.GetResult(typeof(Module4))!;
        await Assert.That(moduleResult.ModuleStatus).IsEqualTo(Status.Successful);
    }
}
