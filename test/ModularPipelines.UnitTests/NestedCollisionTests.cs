using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Exceptions;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;
using TUnit.Assertions;
using TUnit.Core;

namespace ModularPipelines.UnitTests;

public class NestedCollisionTests
{
    [Test]
    public void Modules_Dependent_On_Each_Other_Throws_Exception()
    {
        Assert.That(() => TestPipelineHostBuilder.Create()
                .AddModule<DependencyConflictModule1>()
                .AddModule<DependencyConflictModule2>()
                .AddModule<DependencyConflictModule3>()
                .AddModule<DependencyConflictModule4>()
                .AddModule<DependencyConflictModule5>()
                .ExecutePipelineAsync(),
            Throws.Exception.TypeOf<DependencyCollisionException>()
                .With.Message.EqualTo("Dependency collision detected: **DependencyConflictModule2** -> DependencyConflictModule3 -> DependencyConflictModule4 -> DependencyConflictModule5 -> **DependencyConflictModule2**");
    }

    [DependsOn<DependencyConflictModule2>]
    private class DependencyConflictModule1 : Module
    {
        protected override async Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            return null;
        }
    }

    [DependsOn<DependencyConflictModule3>]
    private class DependencyConflictModule2 : Module
    {
        protected override async Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            return null;
        }
    }

    [DependsOn<DependencyConflictModule4>]
    private class DependencyConflictModule3 : Module
    {
        protected override async Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            return null;
        }
    }

    [DependsOn<DependencyConflictModule5>]
    private class DependencyConflictModule4 : Module
    {
        protected override async Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            return null;
        }
    }

    [DependsOn<DependencyConflictModule2>]
    private class DependencyConflictModule5 : Module
    {
        protected override async Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            return null;
        }
    }
}