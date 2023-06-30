using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Exceptions;
using ModularPipelines.Extensions;
using ModularPipelines.Host;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.UnitTests;

public class NestedCollisionTests
{
    [Test]
    public void Modules_Dependent_On_Each_Other_Throws_Exception()
    {
        Assert.That(() => PipelineHostBuilder.Create()
                .ConfigureServices((context, collection) =>
                {
                    collection.AddModule<DependencyConflictModule1>()
                        .AddModule<DependencyConflictModule2>()
                        .AddModule<DependencyConflictModule3>()
                        .AddModule<DependencyConflictModule4>()
                        .AddModule<DependencyConflictModule5>();
                })
                .ExecutePipelineAsync(), 
            Throws.Exception.TypeOf<DependencyCollisionException>()
                .With.Message.EqualTo("Dependency collision detected: **DependencyConflictModule2** -> DependencyConflictModule3 -> DependencyConflictModule4 -> DependencyConflictModule5 -> **DependencyConflictModule2**"));
    }
    
    [DependsOn<DependencyConflictModule2>]
    private class DependencyConflictModule1 : Module
    {
        protected override async Task<ModuleResult<IDictionary<string, object>>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            return null;
        }
    }
    
    [DependsOn<DependencyConflictModule3>]
    private class DependencyConflictModule2 : Module
    {
        protected override async Task<ModuleResult<IDictionary<string, object>>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            return null;
        }
    }
    
    [DependsOn<DependencyConflictModule4>]
    private class DependencyConflictModule3 : Module
    {
        protected override async Task<ModuleResult<IDictionary<string, object>>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            return null;
        }
    }
    
    [DependsOn<DependencyConflictModule5>]
    private class DependencyConflictModule4 : Module
    {
        protected override async Task<ModuleResult<IDictionary<string, object>>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            return null;
        }
    }
    
    [DependsOn<DependencyConflictModule2>]
    private class DependencyConflictModule5 : Module
    {
        protected override async Task<ModuleResult<IDictionary<string, object>>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            return null;
        }
    }
}