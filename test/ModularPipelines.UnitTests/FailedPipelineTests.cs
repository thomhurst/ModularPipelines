using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Enums;
using ModularPipelines.Modules;
using ModularPipelines.Options;

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

    [DependsOn<Module2>(IgnoreIfNotRegistered = true)]
    private class Module3 : Module
    {
        protected override async Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            _ = GetModuleIfRegistered<Module2>();
            return await NothingAsync();
        }
    }

    [TestCase(ExecutionMode.StopOnFirstException)]
    [TestCase(ExecutionMode.WaitForAllModules)]
    public void Given_Failing_Module_With_Dependent_Module_When_Fail_Fast_Then_Failures_Propagate(ExecutionMode executionMode)
    {
        Assert.That(async () => await TestPipelineHostBuilder.Create()
                .ConfigurePipelineOptions((_, options)
                    => options.ExecutionMode = executionMode)
                .AddModule<Module1>()
                .AddModule<Module2>()
                .AddModule<Module3>()
                .ExecutePipelineAsync(),
            Throws.Exception);
    }

    [TestCase(ExecutionMode.StopOnFirstException)]
    [TestCase(ExecutionMode.WaitForAllModules)]
    public void Given_Failing_Module_When_Fail_Fast_Then_Failures_Propagate(ExecutionMode executionMode)
    {
        Assert.That(async () => await TestPipelineHostBuilder.Create()
                .ConfigurePipelineOptions((_, options)
                    => options.ExecutionMode = executionMode)
                .AddModule<Module1>()
                .AddModule<Module2>()
                .ExecutePipelineAsync(),
            Throws.Exception);
    }

    [TestCase(ExecutionMode.StopOnFirstException)]
    [TestCase(ExecutionMode.WaitForAllModules)]
    public async Task Given_No_Failing_Module_Then_No_Exceptions(ExecutionMode executionMode)
    {
        var pipelineSummary = await TestPipelineHostBuilder.Create()
                .ConfigurePipelineOptions((_, options)
                    => options.ExecutionMode = executionMode)
                .AddModule<Module1>()
                .AddModule<Module3>()
                .ExecutePipelineAsync();

        Assert.That(pipelineSummary.Status, Is.EqualTo(Status.Successful));
    }
}