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

namespace ModularPipelines.UnitTests.Engine;

public class PipelineRequirementTests
{
    [Test]
    public async Task When_Requirement_Succeeds_Then_No_Error()
    {
        var host = await TestPipelineBuilder.Create()
            .AddModule<DummyModule>()
            .AddRequirement<SuccessfulRequirement>()
            .BuildAsync();

        await host.RunAsync();

        var resultRegistry = host.Services.GetRequiredService<IModuleResultRegistry>();
        var result = resultRegistry.GetResult(typeof(DummyModule))!;
        await Assert.That(result.Status).IsEqualTo(ModuleStatus.Succeeded);
    }

    [Test]
    public async Task When_Requirement_Fails_Then_Error()
    {
        var executePipelineDelegate = async () =>
        {
            await TestPipelineBuilder.Create()
                .AddModule<DummyModule>()
                .AddRequirement<FailingRequirement>()
                .RunAsync();
        };

        await Assert.That(executePipelineDelegate)
            .Throws<FailedRequirementsException>()
            .And.HasMessageEqualTo("Requirements failed:\r\nFailingRequirement");
    }

    [Test]
    public async Task When_Requirement_Fails_With_Reason_Then_Error_With_Reason()
    {
        var executePipelineDelegate = async () =>
        {
            await TestPipelineBuilder.Create()
                .AddModule<DummyModule>()
                .AddRequirement<FailingRequirementWithReason>()
                .RunAsync();
        };
        await Assert.That(executePipelineDelegate)
            .Throws<FailedRequirementsException>()
            .And.HasMessageEqualTo("Requirements failed:\r\n" + TestConstants.RequirementErrorMessage);
    }

    private class DummyModule : Module<bool>
    {
        protected internal override async Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return true;
        }
    }

    private class SuccessfulRequirement : IPipelineRequirement
    {
        public async Task<RequirementDecision> MustAsync(IPipelineContext context)
        {
            await Task.Yield();
            return true;
        }
    }

    private class FailingRequirement : IPipelineRequirement
    {
        public async Task<RequirementDecision> MustAsync(IPipelineContext context)
        {
            await Task.Yield();
            return false;
        }
    }

    private class FailingRequirementWithReason : IPipelineRequirement
    {
        public async Task<RequirementDecision> MustAsync(IPipelineContext context)
        {
            await Task.Yield();
            return RequirementDecision.Failed(TestConstants.RequirementErrorMessage);
        }
    }
}
