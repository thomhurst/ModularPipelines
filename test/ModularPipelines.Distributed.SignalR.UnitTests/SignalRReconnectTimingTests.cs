using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.Distributed.SignalR.Coordination;

namespace ModularPipelines.Distributed.SignalR.UnitTests;

public class SignalRReconnectTimingTests
{
    [Test]
    [Arguments(30, 15, 5, 10)]
    [Arguments(20, 15, 5, 0)]
    [Arguments(10, 15, 5, 0)]
    public async Task Recovery_Reserves_Failure_Detection_And_Heartbeat_Cadence(
        int workerTimeout, int peerTimeout, int heartbeatInterval, int expectedBudget)
    {
        var timeout = SignalRDistributedCoordinatorFactory.GetReconnectTimeout(
            new SignalRDistributedOptions { PeerTimeout = TimeSpan.FromSeconds(peerTimeout) },
            new DistributedOptions
            {
                WorkerTimeout = TimeSpan.FromSeconds(workerTimeout),
                WorkerHeartbeatInterval = TimeSpan.FromSeconds(heartbeatInterval),
            });

        await Assert.That(timeout).IsEqualTo(TimeSpan.FromSeconds(expectedBudget));
    }

    [Test]
    public async Task Retry_Policy_Counts_Time_Spent_In_Connection_Attempts()
    {
        var policy = new SignalRDistributedCoordinatorFactory.RetryPolicy(5, TimeSpan.FromSeconds(10));

        // Only one backoff has elapsed, but a slow connection attempt spent the remaining budget.
        var delay = policy.NextRetryDelay(new RetryContext
        {
            PreviousRetryCount = 1,
            ElapsedTime = TimeSpan.FromSeconds(9),
        });

        await Assert.That(delay).IsNull();
    }

    [Test]
    public async Task Retry_Policy_Stops_At_Attempt_Limit()
    {
        var policy = new SignalRDistributedCoordinatorFactory.RetryPolicy(2, TimeSpan.FromMinutes(1));
        var delay = policy.NextRetryDelay(new RetryContext { PreviousRetryCount = 2 });

        await Assert.That(delay).IsNull();
    }

    [Test]
    public async Task Retry_Policy_Preserves_Backoff_Within_Budget()
    {
        var policy = new SignalRDistributedCoordinatorFactory.RetryPolicy(5, TimeSpan.FromSeconds(10));
        var delay = policy.NextRetryDelay(new RetryContext
        {
            PreviousRetryCount = 1,
            ElapsedTime = TimeSpan.FromSeconds(2),
        });

        await Assert.That(delay).IsEqualTo(TimeSpan.FromSeconds(2));
    }

    [Test]
    public async Task Deadline_Cancels_A_Stalled_Recovery_Without_Another_Retry_Callback()
    {
        var stopped = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        using var deadline = new SignalRReconnectDeadline(
            TimeSpan.FromMilliseconds(100),
            () =>
            {
                stopped.SetResult();
                return Task.CompletedTask;
            },
            NullLogger.Instance);
        var token = deadline.Start();
        var stalledRegistration = Task.Delay(Timeout.InfiniteTimeSpan, token);

        await stopped.Task.WaitAsync(TimeSpan.FromSeconds(5));

        await Assert.That(token.IsCancellationRequested).IsTrue();
        await Assert.That(deadline.TryComplete()).IsFalse();
        await Assert.That(async () => await stalledRegistration).Throws<OperationCanceledException>();
    }

    [Test]
    public async Task Repeated_Drops_Share_One_Recovery_Deadline()
    {
        using var deadline = new SignalRReconnectDeadline(
            TimeSpan.FromMinutes(1), () => Task.CompletedTask, NullLogger.Instance);
        var firstToken = deadline.Start();
        var secondToken = deadline.Start();

        await Assert.That(secondToken).IsEqualTo(firstToken);
    }

    [Test]
    public async Task Successful_Recovery_Cancels_Old_Deadline_And_Allows_A_New_Deadline()
    {
        var stopped = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        using var deadline = new SignalRReconnectDeadline(
            TimeSpan.FromMilliseconds(100),
            () =>
            {
                stopped.SetResult();
                return Task.CompletedTask;
            },
            NullLogger.Instance);
        var firstToken = deadline.Start();
        await Assert.That(deadline.TryComplete()).IsTrue();
        var nextToken = deadline.Start();

        await Assert.That(firstToken.IsCancellationRequested).IsTrue();
        await Assert.That(nextToken).IsNotEqualTo(firstToken);
        await stopped.Task.WaitAsync(TimeSpan.FromSeconds(5));
        await Assert.That(nextToken.IsCancellationRequested).IsTrue();
    }

    [Test]
    public async Task Stalled_Worker_Registration_Stops_At_The_Recovery_Deadline()
    {
        await using var test = await SignalRTestMaster.StartAsync(workerTimeout: TimeSpan.FromMilliseconds(20100));
        var worker = await test.ConnectWorkerAsync();
        await worker.RegisterWorkerAsync(SignalRTestMaster.Registration("worker-a"), CancellationToken.None);
        worker.RegisterAgainAsync = _ => Task.Delay(Timeout.InfiniteTimeSpan);

        await worker.OnReconnectingAsync(null);
        await worker.OnReconnectedAsync("stalled").WaitAsync(TimeSpan.FromSeconds(5));

        await Assert.That(await worker.GetMasterStateAsync(CancellationToken.None)).IsEqualTo(DistributedMasterState.Lost);
    }

    [Test]
    public async Task Worker_Stops_Without_Another_Reconnect_Callback()
    {
        await using var test = await SignalRTestMaster.StartAsync(workerTimeout: TimeSpan.FromMilliseconds(20100));
        var worker = await test.ConnectWorkerAsync();
        var registration = SignalRTestMaster.Registration("worker-a");
        await worker.RegisterWorkerAsync(registration, CancellationToken.None);

        await worker.OnReconnectingAsync(null);
        var lease = await worker.DequeueModuleAsync(registration.WorkerId, new HashSet<Capability>(), CancellationToken.None)
            .WaitAsync(TimeSpan.FromSeconds(5));

        await Assert.That(lease).IsNull();
        await Assert.That(await worker.GetMasterStateAsync(CancellationToken.None)).IsEqualTo(DistributedMasterState.Lost);
    }

    [Test]
    public async Task Deadline_During_Transport_Grace_Period_Settles_Worker_Readiness()
    {
        await using var test = await SignalRTestMaster.StartAsync(workerTimeout: TimeSpan.FromMilliseconds(20100));
        var worker = await test.ConnectWorkerAsync();
        var registration = SignalRTestMaster.Registration("worker-a");
        await worker.RegisterWorkerAsync(registration, CancellationToken.None);
        worker.ConnectionChangeGracePeriod = TimeSpan.FromMinutes(1);
        worker.RegisterAgainAsync = _ => Task.FromException(new IOException("connection reset"));

        await worker.OnReconnectingAsync(null);
        await worker.OnReconnectedAsync("interrupted").WaitAsync(TimeSpan.FromSeconds(5));
        var lease = await worker.DequeueModuleAsync(registration.WorkerId, new HashSet<Capability>(), CancellationToken.None)
            .WaitAsync(TimeSpan.FromSeconds(5));

        await Assert.That(lease).IsNull();
    }

    [Test]
    public async Task Older_Registration_Cannot_Complete_A_Newer_Reconnect()
    {
        await using var test = await SignalRTestMaster.StartAsync();
        var worker = await test.ConnectWorkerAsync();
        await worker.RegisterWorkerAsync(SignalRTestMaster.Registration("worker-a"), CancellationToken.None);
        var registerAgain = worker.RegisterAgainAsync;
        var registration = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        worker.RegisterAgainAsync = _ => registration.Task;

        await worker.OnReconnectingAsync(null);
        var firstReconnect = worker.OnReconnectedAsync("first");
        await worker.OnReconnectingAsync(null);
        registration.SetResult();
        await firstReconnect.WaitAsync(TimeSpan.FromSeconds(5));
        var invocation = worker.InvokeAsync(_ => Task.FromResult(true), CancellationToken.None);
        await Assert.That(invocation.IsCompleted).IsFalse();

        worker.RegisterAgainAsync = registerAgain;
        await worker.OnReconnectedAsync("second").WaitAsync(TimeSpan.FromSeconds(5));

        await Assert.That(await invocation.WaitAsync(TimeSpan.FromSeconds(5))).IsTrue();
    }

    [Test]
    public async Task Disposal_Cancels_Recovery_Without_Stopping_An_Already_Disposed_Connection()
    {
        var stops = 0;
        var deadline = new SignalRReconnectDeadline(
            TimeSpan.FromMilliseconds(50),
            () =>
            {
                Interlocked.Increment(ref stops);
                return Task.CompletedTask;
            },
            NullLogger.Instance);
        var token = deadline.Start();
        deadline.Dispose();
        await Task.Delay(TimeSpan.FromMilliseconds(100));

        await Assert.That(token.IsCancellationRequested).IsTrue();
        await Assert.That(stops).IsEqualTo(0);
    }
}
