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
                .With.Message.EqualTo("Dependency collision detected: **ModularPipelines.UnitTests.NestedCollisionTests+DependencyConflictModule5** -> ModularPipelines.UnitTests.NestedCollisionTests+DependencyConflictModule2 -> ModularPipelines.UnitTests.NestedCollisionTests+DependencyConflictModule3 -> ModularPipelines.UnitTests.NestedCollisionTests+DependencyConflictModule4 -> **ModularPipelines.UnitTests.NestedCollisionTests+DependencyConflictModule5**"));
    }
    
    [DependsOn<DependencyConflictModule2>]
    private class DependencyConflictModule1 : Module
    {
        public DependencyConflictModule1(IModuleContext moduleContext) : base(moduleContext)
        {
        }

        protected override async Task<ModuleResult<IDictionary<string, object>>?> ExecuteAsync(CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            return null;
        }
    }
    
    [DependsOn<DependencyConflictModule3>]
    private class DependencyConflictModule2 : Module
    {
        public DependencyConflictModule2(IModuleContext moduleContext) : base(moduleContext)
        {
        }

        protected override async Task<ModuleResult<IDictionary<string, object>>?> ExecuteAsync(CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            return null;
        }
    }
    
    [DependsOn<DependencyConflictModule4>]
    private class DependencyConflictModule3 : Module
    {
        public DependencyConflictModule3(IModuleContext moduleContext) : base(moduleContext)
        {
        }

        protected override async Task<ModuleResult<IDictionary<string, object>>?> ExecuteAsync(CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            return null;
        }
    }
    
    [DependsOn<DependencyConflictModule5>]
    private class DependencyConflictModule4 : Module
    {
        public DependencyConflictModule4(IModuleContext moduleContext) : base(moduleContext)
        {
        }

        protected override async Task<ModuleResult<IDictionary<string, object>>?> ExecuteAsync(CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            return null;
        }
    }
    
    [DependsOn<DependencyConflictModule2>]
    private class DependencyConflictModule5 : Module
    {
        public DependencyConflictModule5(IModuleContext moduleContext) : base(moduleContext)
        {
        }

        protected override async Task<ModuleResult<IDictionary<string, object>>?> ExecuteAsync(CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            return null;
        }
    }
}