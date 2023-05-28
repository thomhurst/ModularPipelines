using Pipeline.NET.Attributes;
using Pipeline.NET.Context;
using Pipeline.NET.Exceptions;
using Pipeline.NET.Host;
using Pipeline.NET.Models;
using Pipeline.NET.Modules;

namespace Pipeline.NET.UnitTests;

public class NestedCollisionTests
{
    [Test]
    public void Modules_Dependent_On_Each_Other_Throws_Exception()
    {
        Assert.That(() => PipelineHostBuilder.Create()
            .AddModule<DependencyConflictModule1>()
            .AddModule<DependencyConflictModule2>()
            .AddModule<DependencyConflictModule3>()
            .AddModule<DependencyConflictModule4>()
            .AddModule<DependencyConflictModule5>()
            .ExecutePipelineAsync(), 
            Throws.Exception.TypeOf<DependencyCollisionException>()
                .With.Message.EqualTo("Dependency collision detected: **Pipeline.NET.UnitTests.NestedCollisionTests+DependencyConflictModule5** -> Pipeline.NET.UnitTests.NestedCollisionTests+DependencyConflictModule2 -> Pipeline.NET.UnitTests.NestedCollisionTests+DependencyConflictModule3 -> Pipeline.NET.UnitTests.NestedCollisionTests+DependencyConflictModule4 -> **Pipeline.NET.UnitTests.NestedCollisionTests+DependencyConflictModule5**"));
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