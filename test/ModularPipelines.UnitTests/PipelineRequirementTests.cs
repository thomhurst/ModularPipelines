using ModularPipelines.Context;
using ModularPipelines.Exceptions;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Requirements;
using ModularPipelines.TestHelpers;
using TUnit.Assertions.AssertConditions.Throws;
using Status = ModularPipelines.Enums.Status;

namespace ModularPipelines.UnitTests;

public class PipelineRequirementTests
{
    [Test]
    public async Task When_Requirement_Succeeds_Then_No_Error()
    {
        var pipelineSummary = await TestPipelineHostBuilder.Create()
            .AddModule<DummyModule>()
            .AddRequirement<SuccessfulRequirement>()
            .ExecutePipelineAsync();

        var dummyModule = pipelineSummary.Modules.OfType<DummyModule>().First();
        await Assert.That(dummyModule.Status).IsEqualTo(Status.Successful);
    }

    [Test]
    public async Task When_Requirement_Fails_Then_Error()
    {
        var executePipelineDelegate = () => TestPipelineHostBuilder.Create()
            .AddModule<DummyModule>()
            .AddRequirement<FailingRequirement>()
            .ExecutePipelineAsync();
        
        await Assert.That(executePipelineDelegate)
            .Throws<FailedRequirementsException>()
            .And.HasMessageEqualTo("Requirements failed:\r\nFailingRequirement");
    }

    [Test]
    public async Task When_Requirement_Fails_With_Reason_Then_Error_With_Reason()
    {
        var executePipelineDelegate = () => TestPipelineHostBuilder.Create()
            .AddModule<DummyModule>()
            .AddRequirement<FailingRequirementWithReason>()
            .ExecutePipelineAsync();
        await Assert.That(executePipelineDelegate)
            .Throws<FailedRequirementsException>()
            .And.HasMessageEqualTo("Requirements failed:\r\nError: Foo bar!");
    }

    private class DummyModule : Module
    {
        protected override async Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return new Dictionary<string, object>();
        }
    }

    private class SuccessfulRequirement : IPipelineRequirement
    {
        public async Task<RequirementDecision> MustAsync(IPipelineHookContext context)
        {
            await Task.Yield();
            return true;
        }
    }

    private class FailingRequirement : IPipelineRequirement
    {
        public async Task<RequirementDecision> MustAsync(IPipelineHookContext context)
        {
            await Task.Yield();
            return false;
        }
    }

    private class FailingRequirementWithReason : IPipelineRequirement
    {
        public async Task<RequirementDecision> MustAsync(IPipelineHookContext context)
        {
            await Task.Yield();
            return RequirementDecision.Failed("Error: Foo bar!");
        }
    }
}