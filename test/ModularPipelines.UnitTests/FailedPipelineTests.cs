using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using ModularPipelines.TestHelpers;
using Status = ModularPipelines.Enums.Status;

namespace ModularPipelines.UnitTests;

public class FailedPipelineTests : TestBase
{
    private class Module1 : Module
    {
        protected override async Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            return await NothingAsync();
        }
    }

    private class Module2 : Module
    {
        protected override Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            throw new Exception();
        }
    }

    [ModularPipelines.Attributes.DependsOn<Module2>(IgnoreIfNotRegistered = true)]
    private class Module3 : Module
    {
        protected override async Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            _ = GetModuleIfRegistered<Module2>();
            return await NothingAsync();
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