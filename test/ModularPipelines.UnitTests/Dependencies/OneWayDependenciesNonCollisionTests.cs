using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests.Dependencies;

public class OneWayDependenciesNonCollisionTests
{
    [Test]
    public async Task Modules_Not_Dependent_On_Each_Other_Succeed()
    {
        await Assert.That(() => TestPipelineHostBuilder.Create()
            .AddModule<DependencyConflictModule1>()
            .AddModule<DependencyConflictModule2>()
            .AddModule<DependencyConflictModule3>()
            .AddModule<DependencyConflictModule4>()
            .AddModule<DependencyConflictModule5>()
            .ExecutePipelineAsync()).ThrowsNothing();
    }

    [ModularPipelines.Attributes.DependsOn<DependencyConflictModule2>]
    private class DependencyConflictModule1 : Module<bool>
    {
        public override async Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            return true;
        }
    }

    [ModularPipelines.Attributes.DependsOn<DependencyConflictModule3>]
    private class DependencyConflictModule2 : Module<bool>
    {
        public override async Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            return true;
        }
    }

    [ModularPipelines.Attributes.DependsOn<DependencyConflictModule4>]
    private class DependencyConflictModule3 : Module<bool>
    {
        public override async Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            return true;
        }
    }

    [ModularPipelines.Attributes.DependsOn<DependencyConflictModule5>]
    private class DependencyConflictModule4 : Module<bool>
    {
        public override async Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            return true;
        }
    }

    private class DependencyConflictModule5 : Module<bool>
    {
        public override async Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            return true;
        }
    }
}