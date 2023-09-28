using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Exceptions;
using ModularPipelines.Extensions;
using ModularPipelines.Modules;

namespace ModularPipelines.UnitTests.Helpers;

public class RunConditionTests : TestBase
{
    [WaitForModuleRunCondition]
    private class Module1 : Module
    {
        protected override Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            return NothingAsync();
        }
    }
    
    [WaitForModuleRunCondition2]
    private class Module2 : Module
    {
        protected override Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            return NothingAsync();
        }
    }

    private class WaitForModuleRunConditionAttribute : RunConditionAttribute
    {
        public override async Task<bool> Condition(IPipelineContext pipelineContext)
        {
            var module = await pipelineContext.GetModule<Module1>()!;
            return true;
        }
    }
    
    private class WaitForModuleRunCondition2Attribute : RunConditionAttribute
    {
        public override Task<bool> Condition(IPipelineContext pipelineContext)
        {
            pipelineContext.GetModule(typeof(Module1));
            return true.AsTask();
        }
    }

    [Test, Timeout(10_000)]
    public void Waiting_On_Module_From_RunConditionAttribute_Does_Not_Hang_And_Throws_Exception()
    {
        Assert.That(async () => await TestPipelineHostBuilder.Create()
            .AddModule<Module1>()
            .ExecutePipelineAsync(),
                Throws.Exception.TypeOf<PipelineException>()
                    .With.Message.EqualTo("Getting a module is not supported outside of another module."));
    }
    
    [Test, Timeout(10_000)]
    public void Waiting_On_Module_From_MandatoryRunConditionAttribute_Does_Not_Hang_And_Throws_Exception()
    {
        Assert.That(async () => await TestPipelineHostBuilder.Create()
                .AddModule<Module2>()
                .ExecutePipelineAsync(),
            Throws.Exception.TypeOf<PipelineException>()
                .With.Message.EqualTo("Getting a module is not supported outside of another module."));
    }
}