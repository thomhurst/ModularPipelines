using System.Collections.Concurrent;
using ModularPipelines.Context;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;
using TUnit.Assertions.Enums;

namespace ModularPipelines.UnitTests.Dependencies;

public class TimedDependencyTests
{
    [Test]
    public async Task DependentModule_WaitsForDependencyToComplete()
    {
        var state = new ExecutionState();
        using var cancellation = new CancellationTokenSource(TestHostSettings.DefaultTestTimeout);
        await using var host = await TestPipelineBuilder.Create()
            .AddModule(new BlockingModule(state))
            .AddModule(new DependentModule(state))
            .BuildAsync();

        var execution = host.RunAsync(cancellation.Token);
        try
        {
            await state.DependencyStarted.Task.WaitAsync(cancellation.Token);
            await Assert.That(state.Events.ToArray()).IsEquivalentTo(["Dependency started"], CollectionOrdering.Matching);
        }
        finally
        {
            state.ReleaseDependency.TrySetResult();
            await execution.WaitAsync(TestHostSettings.DefaultTestTimeout);
        }

        await Assert.That((await execution).Status).IsEqualTo(ModuleStatus.Succeeded);
        await Assert.That(state.Events.ToArray()).IsEquivalentTo(
            ["Dependency started", "Dependency completed", "Dependent started"], CollectionOrdering.Matching);
    }

    private sealed class ExecutionState
    {
        public ConcurrentQueue<string> Events { get; } = new();
        public TaskCompletionSource DependencyStarted { get; } = new(TaskCreationOptions.RunContinuationsAsynchronously);
        public TaskCompletionSource ReleaseDependency { get; } = new(TaskCreationOptions.RunContinuationsAsynchronously);
    }

    private class BlockingModule(ExecutionState state) : Module<bool>
    {
        protected internal override async Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            state.Events.Enqueue("Dependency started");
            state.DependencyStarted.TrySetResult();
            await state.ReleaseDependency.Task.WaitAsync(cancellationToken);
            state.Events.Enqueue("Dependency completed");
            return true;
        }
    }

    [ModularPipelines.DependsOn<BlockingModule>]
    private class DependentModule(ExecutionState state) : Module<bool>
    {
        protected internal override Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            state.Events.Enqueue("Dependent started");
            return Task.FromResult(true);
        }
    }
}
