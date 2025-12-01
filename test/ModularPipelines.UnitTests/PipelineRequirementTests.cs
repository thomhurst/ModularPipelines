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

namespace ModularPipelines.UnitTests;

public class PipelineRequirementTests
{
    [Test]
    public async Task When_Requirement_Succeeds_Then_No_Error()
    {
        var host = await TestPipelineHostBuilder.Create()
            .AddModule<DummyModule>()
            .AddRequirement<SuccessfulRequirement>()
            .BuildHostAsync();

        await host.ExecutePipelineAsync();

        var resultRegistry = host.RootServices.GetRequiredService<IModuleResultRegistry>();
        var result = resultRegistry.GetResult(typeof(DummyModule))!;
        await Assert.That(result.ModuleStatus).IsEqualTo(Status.Successful);
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

    private class DummyModule : IModule<IDictionary<string, object>?>
    {
        public async Task<IDictionary<string, object>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
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