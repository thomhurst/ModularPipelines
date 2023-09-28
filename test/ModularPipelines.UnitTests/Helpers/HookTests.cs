using ModularPipelines.Context;
using ModularPipelines.Exceptions;
using ModularPipelines.Interfaces;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.UnitTests.Helpers;

public class HookTests : TestBase
{
    private class Module1 : Module
    {
        protected override Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            return NothingAsync();
        }
    }

    private class GlobalHooks : IPipelineGlobalHooks
    {
        public async Task OnStartAsync(IPipelineContext pipelineContext)
        {
            await pipelineContext.GetModule<Module1>()!;
        }

        public Task OnEndAsync(IPipelineContext pipelineContext, PipelineSummary pipelineSummary)
        {
            pipelineContext.GetModule(typeof(Module1));
            return Task.CompletedTask;
        }
    }
    
    private class ModuleHooks : IPipelineModuleHooks
    {
        public Task OnBeforeModuleStartAsync(IPipelineContext pipelineContext, ModuleBase module)
        {
            pipelineContext.GetModule(typeof(Module1));
            return Task.CompletedTask;
        }

        public async Task OnAfterModuleEndAsync(IPipelineContext pipelineContext, ModuleBase module)
        {
            await pipelineContext.GetModule<Module1>()!;
        }
    }
    
    
    [Test, Timeout(10_000)]
    public void Waiting_On_Module_From_GlobalHook_Does_Not_Hang_And_Throws_Exception()
    {
        Assert.That(async () => await TestPipelineHostBuilder.Create()
            .AddModule<Module1>()
            .AddGlobalHooks<GlobalHooks>()
            .ExecutePipelineAsync(),
                Throws.Exception.TypeOf<PipelineException>()
                    .With.Message.EqualTo("Getting a module is not supported outside of another module."));
    }
    
    [Test, Timeout(10_000)]
    public void Waiting_On_Module_From_ModuleHook_Does_Not_Hang_And_Throws_Exception()
    {
        Assert.That(async () => await TestPipelineHostBuilder.Create()
                .AddModule<Module1>()
                .AddModuleHooks<ModuleHooks>()
                .ExecutePipelineAsync(),
            Throws.Exception.TypeOf<PipelineException>()
                .With.Message.EqualTo("Getting a module is not supported outside of another module."));
    }
}