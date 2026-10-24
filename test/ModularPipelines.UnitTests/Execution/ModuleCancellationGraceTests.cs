using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Time.Testing;
using ModularPipelines.Configuration;
using ModularPipelines.Engine;
using ModularPipelines.Enums;
using ModularPipelines.Helpers;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests.Execution;

public class ModuleCancellationGraceTests
{
    [Test]
    public async Task Default_Cancellation_Grace_Period_Is_Thirty_Seconds()
    {
        await Assert.That(new PipelineOptions().ModuleCancellationGracePeriod).IsEqualTo(TimeSpan.FromSeconds(30));
    }

    [Test]
    [Arguments(false, false)]
    [Arguments(true, false)]
    [Arguments(false, true)]
    [Arguments(true, true)]
    public async Task Cleanup_Completion_Preserves_External_Cancellation(bool disableTimeout, bool faultDuringCleanup)
    {
        using var cancellation = new CancellationTokenSource();
        var task = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
        var execution = TimeoutHelper.ExecuteWithTimeoutAndDetailsAsync(
            _ => task.Task,
            disableTimeout ? null : Timeout.InfiniteTimeSpan,
            cancellation.Token,
            cancellationGracePeriod: TestHostSettings.DefaultTestTimeout);
        cancellation.Cancel();

        try
        {
            await Task.WhenAny(execution, Task.Delay(TimeSpan.FromMilliseconds(100)));
            await Assert.That(execution.IsCompleted).IsFalse();
        }
        finally
        {
            if (faultDuringCleanup)
            {
                task.TrySetException(new IOException("Cleanup failed."));
            }
            else
            {
                task.TrySetResult(true);
            }
        }

        var exception = await Assert.ThrowsAsync<OperationCanceledException>(async () =>
            await execution.WaitAsync(TestHostSettings.DefaultTestTimeout));
        await Assert.That(exception!.CancellationToken).IsEqualTo(cancellation.Token);
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Cancellation_During_Deadline_Grace_Allows_Pipeline_Cleanup_Budget(bool faultDuringCleanup)
    {
        using var cancellation = new CancellationTokenSource();
        var time = new GraceTimeProvider();
        var task = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
        var execution = TimeoutHelper.ExecuteWithTimeoutAndDetailsAsync(
            _ => task.Task,
            TimeSpan.FromMilliseconds(1),
            cancellation.Token,
            cancellationGracePeriod: TestHostSettings.DefaultTestTimeout,
            timeProvider: time);

        try
        {
            time.Advance(TimeSpan.FromMilliseconds(1));
            await time.DeadlineGraceStarted.Task.WaitAsync(TestHostSettings.DefaultTestTimeout);
            // Keep the deadline grace open regardless of how long the test continuation is queued.
            time.Advance(TimeSpan.FromMilliseconds(100));
            cancellation.Cancel();
            await time.CleanupGraceStarted.Task.WaitAsync(TestHostSettings.DefaultTestTimeout);

            // The original one-second deadline grace must not dispose resources during pipeline cleanup.
            time.Advance(TimeSpan.FromMilliseconds(1100));
            await Assert.That(execution.IsCompleted).IsFalse();
        }
        finally
        {
            if (faultDuringCleanup)
            {
                task.TrySetException(new IOException("Cleanup failed."));
            }
            else
            {
                task.TrySetResult(true);
            }
        }

        var exception = await Assert.ThrowsAsync<OperationCanceledException>(async () =>
            await execution.WaitAsync(TestHostSettings.DefaultTestTimeout));
        await Assert.That(exception!.CancellationToken).IsEqualTo(cancellation.Token);
    }

    [Test]
    public async Task Cancellation_After_Deadline_Grace_Does_Not_Reopen_Cleanup_Budget()
    {
        using var cancellation = new CancellationTokenSource();
        var time = new GraceTimeProvider();
        var task = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
        var execution = TimeoutHelper.ExecuteWithTimeoutAndDetailsAsync(
            _ => task.Task,
            TimeSpan.FromMilliseconds(1),
            cancellation.Token,
            cancellationGracePeriod: TestHostSettings.DefaultTestTimeout,
            timeProvider: time);

        try
        {
            time.Advance(TimeSpan.FromMilliseconds(1));
            await time.DeadlineGraceStarted.Task.WaitAsync(TestHostSettings.DefaultTestTimeout);
            time.Advance(TimeSpan.FromSeconds(1));
            var result = await execution.WaitAsync(TestHostSettings.DefaultTestTimeout);
            cancellation.Cancel();

            await Assert.That(result.TimedOut).IsTrue();
            await Assert.That(result.WasCancellationTokenRespected).IsFalse();
            await Assert.That(time.CleanupGraceStarted.Task.IsCompleted).IsFalse();
        }
        finally
        {
            task.TrySetResult(true);
        }
    }

    private sealed class GraceTimeProvider : FakeTimeProvider
    {
        public TaskCompletionSource DeadlineGraceStarted { get; } = new(TaskCreationOptions.RunContinuationsAsynchronously);
        public TaskCompletionSource CleanupGraceStarted { get; } = new(TaskCreationOptions.RunContinuationsAsynchronously);

        public override ITimer CreateTimer(TimerCallback callback, object? state, TimeSpan dueTime, TimeSpan period)
        {
            var timer = base.CreateTimer(callback, state, dueTime, period);
            if (dueTime == TimeSpan.FromSeconds(1))
            {
                DeadlineGraceStarted.TrySetResult();
            }
            else if (dueTime == TestHostSettings.DefaultTestTimeout)
            {
                CleanupGraceStarted.TrySetResult();
            }

            return timer;
        }
    }

    [Test]
    [Arguments(false, 0)]
    [Arguments(true, 0)]
    [Arguments(false, 100)]
    [Arguments(true, 100)]
    public async Task Unresponsive_Module_Is_Released_After_Configured_Grace(bool disableTimeout, int graceMilliseconds)
    {
        var state = new CleanupState();
        var gracePeriod = TimeSpan.FromMilliseconds(graceMilliseconds);
        var builder = TestPipelineBuilder.Create()
            .ConfigureOptions(options => options with
            {
                DefaultModuleTimeout = disableTimeout ? Timeout.InfiniteTimeSpan : TimeSpan.FromMinutes(30),
                ModuleCancellationGracePeriod = gracePeriod,
            })
            .AddModule<CleaningModule>()
            .AddModule<TeardownModule>();
        builder.Services.AddSingleton(state);
        builder.Services.AddScoped<ScopedResource>();
        await using var pipeline = await builder.BuildAsync();
        var execution = pipeline.RunAsync();

        try
        {
            await state.Started.Task.WaitAsync(TestHostSettings.DefaultTestTimeout);
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            pipeline.Services.GetRequiredService<ModularPipelines.Engine.EngineCancellationToken>().Cancel();
            await execution.WaitAsync(TestHostSettings.DefaultTestTimeout);

            // Allow timer granularity while still rejecting immediate abandonment.
            await Assert.That(stopwatch.Elapsed).IsGreaterThanOrEqualTo(TimeSpan.FromMilliseconds(Math.Max(0, graceMilliseconds - 20)));
            await Assert.That(state.CleanupFinished).IsFalse();
            await Assert.That(state.ModuleDisposed).IsFalse();
            await Assert.That(state.Resource!.IsDisposed).IsTrue();
            await Assert.That(state.TeardownStarted).IsTrue();
        }
        finally
        {
            state.ReleaseCleanup.TrySetResult();
            await state.CleanupCompleted.Task.WaitAsync(TestHostSettings.DefaultTestTimeout);
            await execution.WaitAsync(TestHostSettings.DefaultTestTimeout);
        }

        // Modules belong to the pipeline host; execution scopes own scoped resources.
        await pipeline.DisposeAsync();
        await Assert.That(state.ModuleDisposed).IsTrue();
    }

    [Test]
    [Arguments(false, false)]
    [Arguments(true, false)]
    [Arguments(false, true)]
    [Arguments(true, true)]
    public async Task Cancellation_Waits_For_Cleanup_Before_Disposal_And_AlwaysRun(bool disableTimeout, bool failFast)
    {
        var state = new CleanupState();
        var builder = TestPipelineBuilder.Create()
            .ConfigureOptions(options => options with
            {
                DefaultModuleTimeout = disableTimeout ? Timeout.InfiniteTimeSpan : TimeSpan.FromMinutes(30),
            })
            .AddModule<CleaningModule>()
            .AddModule<TeardownModule>();
        builder.Services.AddSingleton(state);
        builder.Services.AddScoped<ScopedResource>();
        await using var pipeline = await builder.BuildAsync();
        var execution = pipeline.RunAsync();

        try
        {
            await state.Started.Task.WaitAsync(TestHostSettings.DefaultTestTimeout);
            var engineCancellation = pipeline.Services.GetRequiredService<ModularPipelines.Engine.EngineCancellationToken>();
            if (failFast)
            {
                engineCancellation.CancelWithException(new InvalidOperationException("Peer module failed."));
            }
            else
            {
                engineCancellation.Cancel();
            }

            await state.CleanupEntered.Task.WaitAsync(TestHostSettings.DefaultTestTimeout);
            // Give the engine a chance to expose premature completion while cleanup is gated.
            await Task.WhenAny(execution, Task.Delay(TimeSpan.FromMilliseconds(100)));
            await Assert.That(execution.IsCompleted).IsFalse();
            await Assert.That(state.ModuleDisposed).IsFalse();
            await Assert.That(state.Resource!.IsDisposed).IsFalse();
            await Assert.That(state.TeardownStarted).IsFalse();
        }
        finally
        {
            state.ReleaseCleanup.TrySetResult();
            try
            {
                await execution.WaitAsync(TestHostSettings.DefaultTestTimeout);
            }
            catch (InvalidOperationException exception) when (failFast && exception.Message == "Peer module failed.")
            {
                // Fail-fast preserves the original pipeline failure after cleanup.
            }
        }

        await Assert.That(state.CleanupUsedLiveResources).IsTrue();
        await Assert.That(state.ModuleDisposed).IsFalse();
        await Assert.That(state.Resource!.IsDisposed).IsTrue();
        await Assert.That(state.TeardownSawCleanup).IsTrue();
        var result = pipeline.Services.GetRequiredService<IModuleResultRegistry>().GetResult(typeof(CleaningModule));
        await Assert.That(result!.Status).IsEqualTo(ModuleStatus.Canceled);

        await pipeline.DisposeAsync();
        await Assert.That(state.ModuleDisposed).IsTrue();
    }

    public sealed class CleanupState
    {
        public TaskCompletionSource Started { get; } = new(TaskCreationOptions.RunContinuationsAsynchronously);
        public TaskCompletionSource CleanupEntered { get; } = new(TaskCreationOptions.RunContinuationsAsynchronously);
        public TaskCompletionSource ReleaseCleanup { get; } = new(TaskCreationOptions.RunContinuationsAsynchronously);
        public TaskCompletionSource CleanupCompleted { get; } = new(TaskCreationOptions.RunContinuationsAsynchronously);
        public ScopedResource? Resource { get; set; }
        public bool ModuleDisposed { get; set; }
        public bool CleanupUsedLiveResources { get; set; }
        public bool CleanupFinished { get; set; }
        public bool TeardownStarted { get; set; }
        public bool TeardownSawCleanup { get; set; }
    }

    public sealed class ScopedResource : IDisposable
    {
        public bool IsDisposed { get; private set; }
        public void Dispose() => IsDisposed = true;
    }

    public sealed class CleaningModule(CleanupState state) : Module<bool>, IDisposable
    {
        protected internal override async Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            var resource = context.Services.GetRequiredService<ScopedResource>();
            state.Resource = resource;
            state.Started.TrySetResult();
            try
            {
                await Task.Delay(Timeout.InfiniteTimeSpan, cancellationToken);
                return true;
            }
            finally
            {
                state.CleanupEntered.TrySetResult();
                await state.ReleaseCleanup.Task;
                using var registration = cancellationToken.Register(() => { });
                context.Logger.LogInformation("Module cleanup completed.");
                state.CleanupUsedLiveResources = !resource.IsDisposed && !state.ModuleDisposed;
                state.CleanupFinished = true;
                state.CleanupCompleted.TrySetResult();
            }
        }

        public void Dispose() => state.ModuleDisposed = true;
    }

    [DependsOn<CleaningModule>]
    public sealed class TeardownModule(CleanupState state) : Module<bool>
    {
        protected override void Configure(ModuleConfigurationBuilder module) => module.WithAlwaysRun();

        protected internal override Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            state.TeardownStarted = true;
            state.TeardownSawCleanup = state.CleanupFinished;
            return Task.FromResult(true);
        }
    }
}
