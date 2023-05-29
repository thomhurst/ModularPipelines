using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Exceptions;
using ModularPipelines.Extensions;
using ModularPipelines.Host;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.UnitTests;

public class DirectCollisionTests
{
    [Test]
    public void Modules_Dependent_On_Each_Other_Throws_Exception()
    {
        Assert.That(() => PipelineHostBuilder.Create()
                .ConfigureServices((context, collection) =>
                {
                    collection.AddModule<DependencyConflictModule1>()
                        .AddModule<DependencyConflictModule2>();
                })
            .ExecutePipelineAsync(), 
            Throws.Exception.TypeOf<DependencyCollisionException>()
                .With.Message.EqualTo("Dependency collision detected: **ModularPipelines.UnitTests.DirectCollisionTests+DependencyConflictModule2** -> ModularPipelines.UnitTests.DirectCollisionTests+DependencyConflictModule1 -> **ModularPipelines.UnitTests.DirectCollisionTests+DependencyConflictModule2**"));
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