using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Exceptions;
using ModularPipelines.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Requirements;
using ModularPipelines.TestHelpers;
using Status = ModularPipelines.Enums.Status;

namespace ModularPipelines.UnitTests.Requirements;

public class PipelineRequirementBaseClassTests
{
    [Test]
    public async Task Sync_Requirement_With_Pass_Succeeds()
    {
        var host = await TestPipelineHostBuilder.Create()
            .AddModule<DummyModule>()
            .AddRequirement<PassingSyncRequirement>()
            .BuildHostAsync();

        await host.ExecutePipelineAsync();

        var resultRegistry = host.RootServices.GetRequiredService<IModuleResultRegistry>();
        var result = resultRegistry.GetResult(typeof(DummyModule))!;
        await Assert.That(result.ModuleStatus).IsEqualTo(Status.Successful);
    }

    [Test]
    public async Task Sync_Requirement_With_Fail_Throws()
    {
        var executePipelineDelegate = () => TestPipelineHostBuilder.Create()
            .AddModule<DummyModule>()
            .AddRequirement<FailingSyncRequirement>()
            .ExecutePipelineAsync();

        await Assert.That(executePipelineDelegate)
            .Throws<FailedRequirementsException>()
            .And.HasMessageContaining("Sync requirement failed");
    }

    [Test]
    public async Task Async_Requirement_With_Pass_Succeeds()
    {
        var host = await TestPipelineHostBuilder.Create()
            .AddModule<DummyModule>()
            .AddRequirement<PassingAsyncRequirement>()
            .BuildHostAsync();

        await host.ExecutePipelineAsync();

        var resultRegistry = host.RootServices.GetRequiredService<IModuleResultRegistry>();
        var result = resultRegistry.GetResult(typeof(DummyModule))!;
        await Assert.That(result.ModuleStatus).IsEqualTo(Status.Successful);
    }

    [Test]
    public async Task Async_Requirement_With_Fail_Throws()
    {
        var executePipelineDelegate = () => TestPipelineHostBuilder.Create()
            .AddModule<DummyModule>()
            .AddRequirement<FailingAsyncRequirement>()
            .ExecutePipelineAsync();

        await Assert.That(executePipelineDelegate)
            .Throws<FailedRequirementsException>()
            .And.HasMessageContaining("Async requirement failed");
    }

    [Test]
    public async Task When_Helper_With_True_Passes()
    {
        var host = await TestPipelineHostBuilder.Create()
            .AddModule<DummyModule>()
            .AddRequirement<WhenTrueRequirement>()
            .BuildHostAsync();

        await host.ExecutePipelineAsync();

        var resultRegistry = host.RootServices.GetRequiredService<IModuleResultRegistry>();
        var result = resultRegistry.GetResult(typeof(DummyModule))!;
        await Assert.That(result.ModuleStatus).IsEqualTo(Status.Successful);
    }

    [Test]
    public async Task When_Helper_With_False_Fails()
    {
        var executePipelineDelegate = () => TestPipelineHostBuilder.Create()
            .AddModule<DummyModule>()
            .AddRequirement<WhenFalseRequirement>()
            .ExecutePipelineAsync();

        await Assert.That(executePipelineDelegate)
            .Throws<FailedRequirementsException>()
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
        public override async Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return true;
        }
    }

    private class PassingSyncRequirement : PipelineRequirement
    {
        protected override RequirementDecision Must(IPipelineHookContext context) => Pass();
    }

    private class FailingSyncRequirement : PipelineRequirement
    {
        protected override RequirementDecision Must(IPipelineHookContext context) => Fail("Sync requirement failed");
    }

    private class PassingAsyncRequirement : PipelineRequirement
    {
        public override async Task<RequirementDecision> MustAsync(IPipelineHookContext context)
        {
            await Task.Yield();
            return Pass();
        }
    }

    private class FailingAsyncRequirement : PipelineRequirement
    {
        public override async Task<RequirementDecision> MustAsync(IPipelineHookContext context)
        {
            await Task.Yield();
            return Fail("Async requirement failed");
        }
    }

    private class WhenTrueRequirement : PipelineRequirement
    {
        protected override RequirementDecision Must(IPipelineHookContext context)
            => When(true, "Should not see this");
    }

    private class WhenFalseRequirement : PipelineRequirement
    {
        protected override RequirementDecision Must(IPipelineHookContext context)
            => When(false, "When condition failed");
    }

    private class CustomOrderRequirement : PipelineRequirement
    {
        public override int Order => 10;
    }
}
