using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Exceptions;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests;

public class DirectCollisionTests
{
    [Test]
    public async Task Modules_Dependent_On_Each_Other_Throws_Exception()
    {
        await Assert.That(() => TestPipelineHostBuilder.Create()
                .AddModule<DependencyConflictModule1>()
                .AddModule<DependencyConflictModule2>()
            .ExecutePipelineAsync()).
            Throws<DependencyCollisionException>()
                .And.HasMessageEqualTo("Dependency collision detected: **DependencyConflictModule1** -> DependencyConflictModule2 -> **DependencyConflictModule1**");
    }

    [ModularPipelines.Attributes.DependsOn<DependencyConflictModule2>]
    private class DependencyConflictModule1 : Module<bool>
    {
        public override async Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            _ = context.GetModule<DependencyConflictModule2, bool>();
            await Task.Yield();
            return true;
        }
    }

    [ModularPipelines.Attributes.DependsOn<DependencyConflictModule1>]
    private class DependencyConflictModule2 : Module<bool>
    {
        public override async Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            return true;
        }
    }
}