using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Exceptions;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;
using TUnit.Assertions.Extensions.Throws;

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
            ThrowsException().OfType<DependencyCollisionException>()
                .And.ThrowsException().With.Message.EqualTo("Dependency collision detected: **DependencyConflictModule1** -> DependencyConflictModule2 -> **DependencyConflictModule1**");
    }

    [DependsOn<DependencyConflictModule2>]
    private class DependencyConflictModule1 : Module
    {
        protected override async Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await GetModule<DependencyConflictModule2>();
            await Task.CompletedTask;
            return null;
        }
    }

    [DependsOn<DependencyConflictModule1>]
    private class DependencyConflictModule2 : Module
    {
        protected override async Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            return null;
        }
    }
}