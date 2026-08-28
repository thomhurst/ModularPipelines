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

        public bool IsDisposed { get; private set; }

        public async ValueTask DisposeAsync()
        {
            await DisposePipeline!();
            IsDisposed = true;
        }
    }

    private sealed class CrossThreadReentrantDisposalTracker : IDisposable
    {
        public Func<ValueTask>? DisposePipeline { get; set; }

        public bool IsDisposed { get; private set; }

        public void Dispose()
        {
            Task.Run(() => DisposePipeline!().AsTask()).GetAwaiter().GetResult();
            IsDisposed = true;
        }
    }

    private sealed class NonFlowingCrossThreadReentrantDisposalTracker : IDisposable
    {
        public Func<ValueTask>? DisposePipeline { get; set; }

        public bool IsDisposed { get; private set; }

        public void Dispose()
        {
            using (ExecutionContext.SuppressFlow())
            {
                Task.Run(() => DisposePipeline!().AsTask()).GetAwaiter().GetResult();
            }

            IsDisposed = true;
        }
    }

    private sealed class BlockingDisposalTracker : IAsyncDisposable
    {
        public TaskCompletionSource Started { get; } = new(
            TaskCreationOptions.RunContinuationsAsynchronously);

        public TaskCompletionSource Release { get; } = new(
            TaskCreationOptions.RunContinuationsAsynchronously);

        public async ValueTask DisposeAsync()
        {
            Started.TrySetResult();
            await Release.Task;
        }
    }

    private sealed class CapturedContextThrowingDisposalTracker : IAsyncDisposable
    {
        private ExecutionContext? _capturedContext;

        public Func<ValueTask>? DisposePipeline { get; set; }

        public ValueTask DisposeAsync()
        {
            _capturedContext = ExecutionContext.Capture();
            return ValueTask.FromException(new ApplicationException("Captured cleanup failed"));
        }

        public async Task DisposePipelineInCapturedContextAsync()
        {
            ValueTask disposal = default;
            ExecutionContext.Run(
                _capturedContext!,
                _ => disposal = DisposePipeline!(),
                null);
            await disposal;
        }
    }

    private sealed class ThrowingScopedDisposalTracker : IAsyncDisposable
    {
        public ValueTask DisposeAsync() =>
            ValueTask.FromException(new ApplicationException("Scope cleanup failed"));
    }

    private sealed class ThrowingRootDisposalTracker : IAsyncDisposable
    {
        public ValueTask DisposeAsync() =>
            ValueTask.FromException(new ApplicationException("Host cleanup failed"));
    }

    private sealed class ThrowingDisposalTracker : IAsyncDisposable
    {
        public ValueTask DisposeAsync() =>
            ValueTask.FromException(new ApplicationException("Cleanup failed"));
    }

    private sealed class CancelingDisposalTracker : IAsyncDisposable
    {
        public CancellationToken CancellationToken { get; set; }

        public ValueTask DisposeAsync() =>
            ValueTask.FromCanceled(CancellationToken);
    }

    private sealed class ThrowingCleanupInitializer : IInitializer
    {
        public ThrowingCleanupInitializer(ThrowingDisposalTracker disposalTracker)
        {
            ArgumentNullException.ThrowIfNull(disposalTracker);
        }

        public Task InitializeAsync() =>
            Task.FromException(new InvalidOperationException("Startup failed"));
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
        var builder = Pipeline.CreateBuilder();
        builder.AddModule<LifecycleModule>();
        var pipeline = await builder.BuildAsync();

        await pipeline.DisposeAsync();
        await pipeline.DisposeAsync();
    }

    [Test]
    public async Task Reentrant_Disposal_Does_Not_Await_Itself()
    {
        var builder = Pipeline.CreateBuilder();
        builder.AddModule<LifecycleModule>();
        builder.Services.AddSingleton<ReentrantDisposalTracker>();
        var pipeline = await builder.BuildAsync();
        var tracker = pipeline.Services.GetRequiredService<ReentrantDisposalTracker>();
        tracker.DisposePipeline = pipeline.DisposeAsync;

        await pipeline.DisposeAsync().AsTask().WaitAsync(TimeSpan.FromSeconds(5));

        await Assert.That(tracker.IsDisposed).IsTrue();
    }

    [Test]
    public async Task Cross_Thread_Reentrant_Disposal_Does_Not_Deadlock()
    {
        var builder = Pipeline.CreateBuilder();
        builder.AddModule<LifecycleModule>();
        builder.Services.AddSingleton<CrossThreadReentrantDisposalTracker>();
        var pipeline = await builder.BuildAsync();
        var tracker = pipeline.Services.GetRequiredService<CrossThreadReentrantDisposalTracker>();
        tracker.DisposePipeline = pipeline.DisposeAsync;

        await pipeline.DisposeAsync().AsTask().WaitAsync(TimeSpan.FromSeconds(5));

        await Assert.That(tracker.IsDisposed).IsTrue();
    }

    [Test]
    public async Task Non_Flowing_Cross_Thread_Reentrant_Disposal_Does_Not_Deadlock()
    {
        var builder = Pipeline.CreateBuilder();
        builder.AddModule<LifecycleModule>();
        builder.Services.AddSingleton<NonFlowingCrossThreadReentrantDisposalTracker>();
        var pipeline = await builder.BuildAsync();
        var tracker = pipeline.Services
            .GetRequiredService<NonFlowingCrossThreadReentrantDisposalTracker>();
        tracker.DisposePipeline = pipeline.DisposeAsync;

        await pipeline.DisposeAsync().AsTask().WaitAsync(TimeSpan.FromSeconds(5));

        await Assert.That(tracker.IsDisposed).IsTrue();
    }

    [Test]
    public async Task Concurrent_Disposal_Awaits_The_Shared_Task()
    {
        var builder = Pipeline.CreateBuilder();
        builder.AddModule<LifecycleModule>();
        builder.Services.AddSingleton<BlockingDisposalTracker>();
        var pipeline = await builder.BuildAsync();
        var tracker = pipeline.Services
            .GetRequiredService<BlockingDisposalTracker>();

        var firstDisposal = pipeline.DisposeAsync().AsTask();
        await tracker.Started.Task.WaitAsync(TimeSpan.FromSeconds(5));
        var concurrentDisposal = pipeline.DisposeAsync().AsTask();

        await Assert.That(concurrentDisposal.IsCompleted).IsFalse();
        tracker.Release.TrySetResult();
        await Task.WhenAll(firstDisposal, concurrentDisposal)
            .WaitAsync(TimeSpan.FromSeconds(5));
    }

    [Test]
    public async Task Captured_Disposal_Context_Observes_Completed_Failure()
    {
        var builder = Pipeline.CreateBuilder();
        builder.AddModule<LifecycleModule>();
        builder.Services.AddSingleton<CapturedContextThrowingDisposalTracker>();
        var pipeline = await builder.BuildAsync();
        var tracker = pipeline.Services
            .GetRequiredService<CapturedContextThrowingDisposalTracker>();
        tracker.DisposePipeline = pipeline.DisposeAsync;

        _ = await Assert.ThrowsAsync<ApplicationException>(
            () => pipeline.DisposeAsync().AsTask());
        var repeatedException = await Assert.ThrowsAsync<ApplicationException>(
            tracker.DisposePipelineInCapturedContextAsync);

        await Assert.That(repeatedException!.Message).IsEqualTo("Captured cleanup failed");
    }

    [Test]
    public async Task Disposal_Aggregates_Scope_And_Host_Failures()
    {
        var builder = Pipeline.CreateBuilder();
        builder.AddModule<LifecycleModule>();
        builder.Services.AddScoped<ThrowingScopedDisposalTracker>();
        builder.Services.AddSingleton<ThrowingRootDisposalTracker>();
        var pipeline = await builder.BuildAsync();
        _ = pipeline.Services.GetRequiredService<ThrowingScopedDisposalTracker>();
        _ = pipeline.Services.GetRequiredService<ThrowingRootDisposalTracker>();

        var exception = await Assert.ThrowsAsync<AggregateException>(
            () => pipeline.DisposeAsync().AsTask());
        var messages = exception!.Flatten().InnerExceptions
            .Select(innerException => innerException.Message)
            .ToArray();

        using (Assert.Multiple())
        {
            await Assert.That(messages).Contains("Scope cleanup failed");
            await Assert.That(messages).Contains("Host cleanup failed");
        }
    }

    [Test]
    public async Task Canceled_Disposal_Produces_A_Canceled_Task()
    {
        using var cancellationTokenSource = new CancellationTokenSource();
        var builder = Pipeline.CreateBuilder();
        builder.AddModule<LifecycleModule>();
        builder.Services.AddSingleton<CancelingDisposalTracker>();
        var pipeline = await builder.BuildAsync();
        var tracker = pipeline.Services.GetRequiredService<CancelingDisposalTracker>();
        tracker.CancellationToken = cancellationTokenSource.Token;
        cancellationTokenSource.Cancel();

        var disposalTask = pipeline.DisposeAsync().AsTask();

        await Assert.ThrowsAsync<OperationCanceledException>(() => disposalTask);
        await Assert.That(disposalTask.IsCanceled).IsTrue();
    }

    [Test]
    public async Task Initialization_Failure_Disposes_The_Built_Host()
    {
        DisposalTracker? disposalTracker = null;
        var builder = Pipeline.CreateBuilder();
        builder.AddModule<LifecycleModule>();
        builder.Services.AddSingleton(_ => disposalTracker = new DisposalTracker());
        builder.Services.AddSingleton<IInitializer, ThrowingInitializer>();

        await Assert.ThrowsAsync<InvalidOperationException>(() => builder.BuildAsync());

        await Assert.That(disposalTracker).IsNotNull();
        await Assert.That(disposalTracker!.IsDisposed).IsTrue();
    }

    [Test]
    public async Task Initialization_Failure_Is_Preserved_When_Disposal_Also_Fails()
    {
        var builder = Pipeline.CreateBuilder();
        builder.AddModule<LifecycleModule>();
        builder.Services.AddSingleton<ThrowingDisposalTracker>();
        builder.Services.AddSingleton<IInitializer, ThrowingCleanupInitializer>();

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => builder.BuildAsync());

        await Assert.That(exception).IsNotNull();
        await Assert.That(exception!.Message).IsEqualTo("Startup failed");
        await Assert.That(exception.Data.Values.OfType<ApplicationException>().Single().Message)
            .IsEqualTo("Cleanup failed");
    }
}
