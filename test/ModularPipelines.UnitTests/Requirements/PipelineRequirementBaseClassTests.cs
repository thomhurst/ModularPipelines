using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Exceptions;
using ModularPipelines.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Requirements;
using ModularPipelines.TestHelpers;
using ModularPipelines.Enums;

namespace ModularPipelines.UnitTests.Requirements;

public class PipelineRequirementBaseClassTests
{
    [Test]
    public async Task Sync_Requirement_With_Pass_Succeeds()
    {
        var host = await TestPipelineBuilder.Create()
            .AddModule<DummyModule>()
            .AddRequirement<PassingSyncRequirement>()
            .BuildAsync();

        await host.RunAsync();

        var resultRegistry = host.Services.GetRequiredService<IModuleResultRegistry>();
        var result = resultRegistry.GetResult(typeof(DummyModule))!;
        await Assert.That(result.Status).IsEqualTo(ModuleStatus.Succeeded);
    }

    [Test]
    public async Task Sync_Requirement_With_Fail_Throws()
    {
        var executePipelineDelegate = async () =>
        {
            await TestPipelineBuilder.Create()
                .AddModule<DummyModule>()
                .AddRequirement<FailingSyncRequirement>()
                .RunAsync();
        };

        await Assert.That(executePipelineDelegate)
            .Throws<RequirementNotMetException>()
            .And.HasMessageContaining("Sync requirement failed");
    }

    [Test]
    public async Task Async_Requirement_With_Pass_Succeeds()
    {
        var host = await TestPipelineBuilder.Create()
            .AddModule<DummyModule>()
            .AddRequirement<PassingAsyncRequirement>()
            .BuildAsync();

        await host.RunAsync();

        var resultRegistry = host.Services.GetRequiredService<IModuleResultRegistry>();
        var result = resultRegistry.GetResult(typeof(DummyModule))!;
        await Assert.That(result.Status).IsEqualTo(ModuleStatus.Succeeded);
    }

    [Test]
    public async Task Async_Requirement_With_Fail_Throws()
    {
        var executePipelineDelegate = async () =>
        {
            await TestPipelineBuilder.Create()
                .AddModule<DummyModule>()
                .AddRequirement<FailingAsyncRequirement>()
                .RunAsync();
        };

        await Assert.That(executePipelineDelegate)
            .Throws<RequirementNotMetException>()
            .And.HasMessageContaining("Async requirement failed");
    }

    [Test]
    public async Task When_Helper_With_True_Passes()
    {
        var host = await TestPipelineBuilder.Create()
            .AddModule<DummyModule>()
            .AddRequirement<WhenTrueRequirement>()
            .BuildAsync();

        await host.RunAsync();

        var resultRegistry = host.Services.GetRequiredService<IModuleResultRegistry>();
        var result = resultRegistry.GetResult(typeof(DummyModule))!;
        await Assert.That(result.Status).IsEqualTo(ModuleStatus.Succeeded);
    }

    [Test]
    public async Task When_Helper_With_False_Fails()
    {
        var executePipelineDelegate = async () =>
        {
            await TestPipelineBuilder.Create()
                .AddModule<DummyModule>()
                .AddRequirement<WhenFalseRequirement>()
                .RunAsync();
        };

        await Assert.That(executePipelineDelegate)
            .Throws<RequirementNotMetException>()
            .And.HasMessageContaining("When condition failed");
    }

    [Test]
    public async Task Custom_Order_Is_Respected()
    {
        var requirement = new CustomOrderRequirement();
        await Assert.That(requirement.Order).IsEqualTo(10);
    }

    private class DummyModule : Module<bool>
    {
        protected internal override async Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return true;
        }
    }

    private class PassingSyncRequirement : PipelineRequirement
    {
        public override Task<RequirementDecision> EvaluateAsync(IPipelineContext context, CancellationToken cancellationToken)
            => Task.FromResult(Pass());
    }

    private class FailingSyncRequirement : PipelineRequirement
    {
        public override Task<RequirementDecision> EvaluateAsync(IPipelineContext context, CancellationToken cancellationToken)
            => Task.FromResult(Fail("Sync requirement failed"));
    }

    private class PassingAsyncRequirement : PipelineRequirement
    {
        public override async Task<RequirementDecision> EvaluateAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return Pass();
        }
    }

    private class FailingAsyncRequirement : PipelineRequirement
    {
        public override async Task<RequirementDecision> EvaluateAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return Fail("Async requirement failed");
        }
    }

    private class WhenTrueRequirement : PipelineRequirement
    {
        public override Task<RequirementDecision> EvaluateAsync(IPipelineContext context, CancellationToken cancellationToken)
            => Task.FromResult(When(true, "Should not see this"));
    }

    private class WhenFalseRequirement : PipelineRequirement
    {
        public override Task<RequirementDecision> EvaluateAsync(IPipelineContext context, CancellationToken cancellationToken)
            => Task.FromResult(When(false, "When condition failed"));
    }

    private class CustomOrderRequirement : PipelineRequirement
    {
        public override int Order => 10;
    }
}
