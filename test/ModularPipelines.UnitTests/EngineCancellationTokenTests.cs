using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Enums;
using ModularPipelines.Extensions;
using ModularPipelines.Modules;

namespace ModularPipelines.UnitTests;

public class EngineCancellationTokenTests : TestBase
{
    private class BadModule : Module
    {
        protected override async Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            
            // Exception will cause the engine to cancel the engine token
            throw new Exception();
        }
    }
    
    [DependsOn<BadModule>]
    private class Module1 : Module
    {
        protected override Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            return NothingAsync();
        }
    }
    
    private class LongRunningModule : Module
    {
        protected override async Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await Task.Delay(TimeSpan.FromSeconds(30), cancellationToken);
            return await NothingAsync();
        }
    }

    [Test, Repeat(10)]
    public async Task When_Cancel_Engine_Token_With_DependsOn_Then_Modules_Cancel()
    {
        var host = await TestPipelineHostBuilder.Create()
            .ConfigureServices((_, collection) =>
            {
                collection.AddLogging(cfg => cfg.SetMinimumLevel(LogLevel.Debug));
            })
            .AddModule<BadModule>()
            .AddModule<Module1>()
            .BuildHostAsync();
        
        var modules = host.RootServices.GetServices<ModuleBase>();

        var module1 = modules.GetModule<Module1>();
        
        var pipelineTask = host.ExecutePipelineAsync();
        
        Assert.That(async () => await pipelineTask, Throws.Exception);
        
        Assert.That(module1.Status, Is.EqualTo(Status.NotYetStarted));
    }

    [Test, Repeat(10)]
    public async Task When_Cancel_Engine_Token_Without_DependsOn_Then_Modules_Cancel()
    {
        var host = await TestPipelineHostBuilder.Create()
            .AddModule<BadModule>()
            .AddModule<LongRunningModule>()
            .BuildHostAsync();

        var modules = host.RootServices.GetServices<ModuleBase>();

        var longRunningModule = modules.GetModule<LongRunningModule>();

        var pipelineTask = host.ExecutePipelineAsync();
        
        Assert.That(async () => await pipelineTask, Throws.Exception);
        
        Assert.That(longRunningModule.Status, Is.EqualTo(Status.PipelineTerminated));
    }
}