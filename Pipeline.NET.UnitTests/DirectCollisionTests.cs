using Pipeline.NET.Attributes;
using Pipeline.NET.Context;
using Pipeline.NET.Exceptions;
using Pipeline.NET.Host;
using Pipeline.NET.Models;
using Pipeline.NET.Modules;

namespace Pipeline.NET.UnitTests;

public class DirectCollisionTests
{
    [Test]
    public void Modules_Dependent_On_Each_Other_Throws_Exception()
    {
        Assert.That(() => PipelineHostBuilder.Create()
            .AddModule<DependencyConflictModule1>()
            .AddModule<DependencyConflictModule2>()
            .ExecutePipelineAsync(), 
            Throws.Exception.TypeOf<DependencyCollisionException>()
                .With.Message.EqualTo("Dependency collision detected: **Pipeline.NET.UnitTests.DirectCollisionTests+DependencyConflictModule2** -> Pipeline.NET.UnitTests.DirectCollisionTests+DependencyConflictModule1 -> **Pipeline.NET.UnitTests.DirectCollisionTests+DependencyConflictModule2**"));
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
    
    [DependsOn<DependencyConflictModule1>]
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
}