using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using ModularPipelines.TestHelpers;
using ModularPipelines.Enums;

namespace ModularPipelines.UnitTests.Execution;

public class FailedPipelineTests : TestBase
{
    private class Module1 : Module<bool>
    {
        protected internal override async Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return true;
        }
    }

    private class Module2 : Module<bool>
    {
        protected internal override Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            throw new Exception();
        }
    }

    [ModularPipelines.DependsOn<Module2>(Optional = true)]
    private class Module3 : Module<bool>
    {
        protected internal override async Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            _ = context.GetModuleIfRegistered<Module2>();
            await Task.Yield();
            return true;
        }
    }

    [Test]
    [Arguments(FailureMode.FailFast)]
    [Arguments(FailureMode.ContinueOnFailure)]
    public async Task Given_Failing_Module_With_Dependent_Module_Then_Failures_Propagate(FailureMode failureMode)
    {
        await Assert.That(async () => await TestPipelineBuilder.Create()
                .ConfigureOptions(options => options with
                {
                    FailureMode = failureMode,
                    ThrowOnPipelineFailure = true,
                })
                .AddModule<Module1>()
                .AddModule<Module2>()
                .AddModule<Module3>()
                .RunAsync()).ThrowsException()
            ;
    }

    [Test]
    [Arguments(FailureMode.FailFast)]
    [Arguments(FailureMode.ContinueOnFailure)]
    public async Task Given_Failing_Module_Then_Failures_Propagate(FailureMode failureMode)
    {
        await Assert.That(async () => await TestPipelineBuilder.Create()
                .ConfigureOptions(options => options with
                {
                    FailureMode = failureMode,
                    ThrowOnPipelineFailure = true,
                })
                .AddModule<Module1>()
                .AddModule<Module2>()
                .RunAsync()).
            ThrowsException();
    }

    [Test]
    [Arguments(FailureMode.FailFast)]
    [Arguments(FailureMode.ContinueOnFailure)]
    public async Task Given_No_Failing_Module_Then_No_Exceptions(FailureMode failureMode)
    {
        var pipelineSummary = await TestPipelineBuilder.Create()
                .ConfigureOptions(options => options with
                {
                    FailureMode = failureMode,
                })
                .AddModule<Module1>()
                .AddModule<Module3>()
                .RunAsync();
        await Assert.That(pipelineSummary.Status).IsEqualTo(ModuleStatus.Succeeded);
    }
}
