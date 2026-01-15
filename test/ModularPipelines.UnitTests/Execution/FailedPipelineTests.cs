using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using ModularPipelines.TestHelpers;
using Status = ModularPipelines.Enums.Status;

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

    [ModularPipelines.Attributes.DependsOn<Module2>(IgnoreIfNotRegistered = true)]
    private class Module3 : Module<bool>
    {
        protected internal override async Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            _ = context.GetModuleIfRegistered<Module2, bool>();
            await Task.Yield();
            return true;
        }
    }

    [Test]
    [Arguments(ExecutionMode.StopOnFirstException)]
    [Arguments(ExecutionMode.WaitForAllModules)]
    public async Task Given_Failing_Module_With_Dependent_Module_When_Fail_Fast_Then_Failures_Propagate(ExecutionMode executionMode)
    {
        await Assert.That(async () => await TestPipelineHostBuilder.Create()
                .ConfigurePipelineOptions((_, options)
                    => options.ExecutionMode = executionMode)
                .AddModule<Module1>()
                .AddModule<Module2>()
                .AddModule<Module3>()
                .ExecutePipelineAsync()).ThrowsException()
            ;
    }

    [Test]
    [Arguments(ExecutionMode.StopOnFirstException)]
    [Arguments(ExecutionMode.WaitForAllModules)]
    public async Task Given_Failing_Module_When_Fail_Fast_Then_Failures_Propagate(ExecutionMode executionMode)
    {
        await Assert.That(async () => await TestPipelineHostBuilder.Create()
                .ConfigurePipelineOptions((_, options)
                    => options.ExecutionMode = executionMode)
                .AddModule<Module1>()
                .AddModule<Module2>()
                .ExecutePipelineAsync()).
            ThrowsException();
    }

    [Test]
    [Arguments(ExecutionMode.StopOnFirstException)]
    [Arguments(ExecutionMode.WaitForAllModules)]
    public async Task Given_No_Failing_Module_Then_No_Exceptions(ExecutionMode executionMode)
    {
        var pipelineSummary = await TestPipelineHostBuilder.Create()
                .ConfigurePipelineOptions((_, options)
                    => options.ExecutionMode = executionMode)
                .AddModule<Module1>()
                .AddModule<Module3>()
                .ExecutePipelineAsync();
        await Assert.That(pipelineSummary.Status).IsEqualTo(Status.Successful);
    }
}