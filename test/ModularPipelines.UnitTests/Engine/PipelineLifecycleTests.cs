using System.Reflection;
using Initialization.Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Context;
using ModularPipelines.Modules;

namespace ModularPipelines.UnitTests.Engine;

public class PipelineLifecycleTests
{
    private sealed class LifecycleModule : Module<string>
    {
        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            Task.FromResult<string?>("lifecycle");
    }

    private sealed class ThrowingInitializer : IInitializer
    {
        public ThrowingInitializer(DisposalTracker disposalTracker)
        {
            ArgumentNullException.ThrowIfNull(disposalTracker);
        }

        public Task InitializeAsync() =>
            Task.FromException(new InvalidOperationException("Initialization failed"));
    }

    private sealed class DisposalTracker : IAsyncDisposable
    {
        public bool IsDisposed { get; private set; }

        public ValueTask DisposeAsync()
        {
            IsDisposed = true;
            return ValueTask.CompletedTask;
        }
    }

    private sealed class ReentrantDisposalTracker : IAsyncDisposable
    {
        public Func<ValueTask>? DisposePipeline { get; set; }

        public Task? ReentrantDisposalTask { get; private set; }

        public ValueTask DisposeAsync()
        {
            ReentrantDisposalTask = DisposePipeline!().AsTask();
            return ValueTask.CompletedTask;
        }
    }

    [Test]
    public async Task Pipeline_Does_Not_Declare_A_Finalizer()
    {
        var finalizer = typeof(PipelineImpl).GetMethod(
            "Finalize",
            BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);

        await Assert.That(finalizer).IsNull();
    }

    [Test]
    public async Task Pipeline_Disposal_Is_Idempotent()
    {
        using var builder = Pipeline.CreateBuilder();
        builder.AddModule<LifecycleModule>();
        var pipeline = await builder.BuildAsync();

        await pipeline.DisposeAsync();
        await pipeline.DisposeAsync();
    }

    [Test]
    public async Task Reentrant_Disposal_Observes_The_Published_Task()
    {
        using var builder = Pipeline.CreateBuilder();
        builder.AddModule<LifecycleModule>();
        builder.Services.AddSingleton<ReentrantDisposalTracker>();
        var pipeline = await builder.BuildAsync();
        var tracker = pipeline.Services.GetRequiredService<ReentrantDisposalTracker>();
        tracker.DisposePipeline = pipeline.DisposeAsync;

        var disposalTask = pipeline.DisposeAsync().AsTask();
        await disposalTask;

        await Assert.That(tracker.ReentrantDisposalTask).IsNotNull();
        await Assert.That(tracker.ReentrantDisposalTask!).IsSameReferenceAs(disposalTask);
    }

    [Test]
    public async Task Initialization_Failure_Disposes_The_Built_Host()
    {
        DisposalTracker? disposalTracker = null;
        using var builder = Pipeline.CreateBuilder();
        builder.AddModule<LifecycleModule>();
        builder.Services.AddSingleton(_ => disposalTracker = new DisposalTracker());
        builder.Services.AddSingleton<IInitializer, ThrowingInitializer>();

        await Assert.ThrowsAsync<InvalidOperationException>(() => builder.BuildAsync());

        await Assert.That(disposalTracker).IsNotNull();
        await Assert.That(disposalTracker!.IsDisposed).IsTrue();
    }
}
