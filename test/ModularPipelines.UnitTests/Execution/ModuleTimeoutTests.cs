using ModularPipelines.Configuration;
using ModularPipelines.Context;
using ModularPipelines.Exceptions;
using ModularPipelines.Helpers;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests.Execution;

public class ModuleTimeoutTests : TestBase
{
    private class Module_UsingCancellationToken : Module<string>
    {
        private static readonly TaskCompletionSource<bool> _taskCompletionSource = new();

        protected override void Configure(ModuleConfigurationBuilder module) => module
            .WithTimeout(TimeSpan.FromSeconds(1));

        protected internal override async Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await _taskCompletionSource.Task.WaitAsync(cancellationToken);
            return TestConstants.TestString;
        }
    }

    private class Module_NotUsingCancellationToken : Module<string>
    {
        protected override void Configure(ModuleConfigurationBuilder module) => module
            .WithTimeout(TimeSpan.FromSeconds(1));

        protected internal override async Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Delay(TimeSpan.FromSeconds(5));
            return TestConstants.TestString;
        }
    }

    private class NoTimeoutModule : Module<string>
    {
        protected internal override async Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(10), cancellationToken);
            return TestConstants.TestString;
        }
    }

    private class PipelineDefaultTimeoutModule : Module<string>
    {
        protected internal override async Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Delay(Timeout.InfiniteTimeSpan, cancellationToken);
            return TestConstants.TestString;
        }
    }

    private class ZeroDefaultTimeoutModule : Module<string>
    {
        protected internal override async Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(100), cancellationToken);
            return TestConstants.TestString;
        }
    }

    [Test]
    public async Task Default_Module_Timeout_Is_Thirty_Minutes()
    {
        var options = new PipelineOptions();

        await Assert.That(options.DefaultModuleTimeout).IsEqualTo(TimeSpan.FromMinutes(30));
    }

    [Test]
    public async Task Default_AlwaysRun_Progress_Timeout_Is_Thirty_Seconds()
    {
        var options = new PipelineOptions();

        await Assert.That(options.AlwaysRunProgressTimeout).IsEqualTo(TimeSpan.FromSeconds(30));
    }

    [Test]
    public async Task Pipeline_Default_Module_Timeout_Is_Applied()
    {
        var exception = await Assert.ThrowsAsync<ModuleFailedException>(async () => await TestPipelineBuilder.Create()
            .ConfigureOptions(options => options with
            {
                DefaultModuleTimeout = TimeSpan.FromMilliseconds(10),
            })
            .AddModule<PipelineDefaultTimeoutModule>()
            .RunAsync()
            .WaitAsync(TestHostSettings.DefaultTestTimeout));

        var timeoutException = exception!.InnerException as ModuleTimeoutException;
        await Assert.That(timeoutException).IsNotNull();
        await Assert.That(timeoutException!.ConfiguredTimeout).IsEqualTo(TimeSpan.FromMilliseconds(10));
    }

    [Test]
    public async Task Zero_Pipeline_Default_Module_Timeout_Disables_Timeout()
    {
        await Assert.That(async () => await TestPipelineBuilder.Create()
            .ConfigureOptions(options => options with
            {
                DefaultModuleTimeout = TimeSpan.Zero,
            })
            .AddModule<ZeroDefaultTimeoutModule>()
            .RunAsync()).ThrowsNothing();
    }

    [Test]
    public async Task Throws_TaskException_When_Using_CancellationToken()
    {
        var exception = await Assert.ThrowsAsync<ModuleFailedException>(RunModule<Module_UsingCancellationToken>);

        var innerException = exception!.InnerException;
        var isExpectedType = innerException is ModuleTimeoutException or TaskCanceledException;
        await Assert.That(isExpectedType).IsTrue();
    }

    [Test]
    public async Task Throws_Timeout_Exception_When_Not_Using_CancellationToken()
    {
        var exception = await Assert.ThrowsAsync<ModuleFailedException>(RunModule<Module_NotUsingCancellationToken>);

        var innerException = exception!.InnerException;
        var isExpectedType = innerException is ModuleTimeoutException or OperationCanceledException or TaskCanceledException;
        await Assert.That(isExpectedType).IsTrue();
    }

    [Test]
    public async Task No_Timeout_Does_Not_Throw_Exception()
    {
        await Assert.That(async () => { await RunModule<NoTimeoutModule>(); }).ThrowsNothing();
    }

    [Test]
    public async Task Timeout_Exception_Contains_Configured_Timeout()
    {
        var exception = await Assert.ThrowsAsync<ModuleFailedException>(RunModule<Module_UsingCancellationToken>);

        var timeoutException = exception!.InnerException as ModuleTimeoutException;
        await Assert.That(timeoutException).IsNotNull();
        await Assert.That(timeoutException!.ConfiguredTimeout).IsEqualTo(TimeSpan.FromSeconds(1));
    }

    [Test]
    public async Task Timeout_Exception_Contains_Module_Type()
    {
        var exception = await Assert.ThrowsAsync<ModuleFailedException>(RunModule<Module_UsingCancellationToken>);

        var timeoutException = exception!.InnerException as ModuleTimeoutException;
        await Assert.That(timeoutException).IsNotNull();
        await Assert.That(timeoutException!.ModuleType).IsEqualTo(typeof(Module_UsingCancellationToken));
    }

    [Test]
    public async Task Timeout_Exception_Contains_Elapsed_Time()
    {
        var exception = await Assert.ThrowsAsync<ModuleFailedException>(RunModule<Module_UsingCancellationToken>);

        var timeoutException = exception!.InnerException as ModuleTimeoutException;
        await Assert.That(timeoutException).IsNotNull();

        // Elapsed time should be at least close to the configured timeout
        await Assert.That(timeoutException!.ElapsedTime).IsGreaterThanOrEqualTo(TimeSpan.FromMilliseconds(900));
    }

    [Test]
    public async Task Timeout_Exception_Indicates_Token_Was_Respected_When_Module_Uses_Token()
    {
        var exception = await Assert.ThrowsAsync<ModuleFailedException>(RunModule<Module_UsingCancellationToken>);

        var timeoutException = exception!.InnerException as ModuleTimeoutException;
        await Assert.That(timeoutException).IsNotNull();
        await Assert.That(timeoutException!.WasCancellationTokenRespected).IsTrue();
    }

    [Test]
    public async Task Timeout_Exception_Indicates_Token_Was_Not_Respected_When_Module_Ignores_Token()
    {
        var exception = await Assert.ThrowsAsync<ModuleFailedException>(RunModule<Module_NotUsingCancellationToken>);

        var timeoutException = exception!.InnerException as ModuleTimeoutException;
        await Assert.That(timeoutException).IsNotNull();
        await Assert.That(timeoutException!.WasCancellationTokenRespected).IsFalse();
    }

    [Test]
    public async Task Timeout_Exception_Message_Reports_Grace_Period_Expiry()
    {
        var exception = await Assert.ThrowsAsync<ModuleFailedException>(RunModule<Module_NotUsingCancellationToken>);

        var timeoutException = exception!.InnerException as ModuleTimeoutException;
        await Assert.That(timeoutException).IsNotNull();
        await Assert.That(timeoutException!.Message).Contains("did not complete within the cancellation grace period");
    }

    [Test]
    public async Task Timeout_Fault_During_Grace_Period_Counts_As_Response()
    {
        for (var iteration = 0; iteration < 25; iteration++)
        {
            var result = await TimeoutHelper.ExecuteWithTimeoutAndDetailsAsync(
                async cancellationToken =>
                {
                    var completion = new TaskCompletionSource<bool>(
                        TaskCreationOptions.RunContinuationsAsynchronously);
                    using var registration = cancellationToken.Register(
                        static state => ((TaskCompletionSource<bool>) state!).TrySetException(
                            new TimeoutException("Inner operation timed out.")),
                        completion);
                    return await completion.Task.ConfigureAwait(false);
                },
                TimeSpan.FromMilliseconds(10),
                CancellationToken.None);

            using (Assert.Multiple())
            {
                await Assert.That(result.TimedOut).IsTrue();
                await Assert.That(result.WasCancellationTokenRespected).IsTrue();
            }
        }
    }

    [Test]
    public async Task Completed_Factory_Task_Wins_When_Factory_Raises_External_Cancellation()
    {
        using var externalCancellation = new CancellationTokenSource();
        var result = await TimeoutHelper.ExecuteWithTimeoutAndDetailsAsync(
            _ =>
            {
                externalCancellation.Cancel();
                return Task.FromResult(true);
            },
            TimeSpan.FromSeconds(10),
            externalCancellation.Token);

        await Assert.That(result.Value).IsTrue();
        await Assert.That(result.TimedOut).IsFalse();
    }

    [Test]
    public async Task Timeout_Does_Not_Claim_Unrelated_Cancellation_Before_Deadline()
    {
        using var unrelatedCancellation = new CancellationTokenSource();
        unrelatedCancellation.Cancel();

        var exception = await Assert.ThrowsAsync<OperationCanceledException>(async () =>
            await TimeoutHelper.ExecuteWithTimeoutAndDetailsAsync(
                _ => Task.FromCanceled<bool>(unrelatedCancellation.Token),
                TimeSpan.FromSeconds(1),
                CancellationToken.None));

        await Assert.That(exception!.CancellationToken).IsEqualTo(unrelatedCancellation.Token);
    }

    [Test]
    public async Task Timeout_Claims_Cancellation_Through_Linked_Attempt_Token()
    {
        using var unrelatedCancellation = new CancellationTokenSource();

        var result = await TimeoutHelper.ExecuteWithTimeoutAndDetailsAsync(
            async timeoutToken =>
            {
                using var linkedCancellation = CancellationTokenSource.CreateLinkedTokenSource(
                    timeoutToken,
                    unrelatedCancellation.Token);
                await Task.Delay(Timeout.InfiniteTimeSpan, linkedCancellation.Token);
                return true;
            },
            TimeSpan.FromMilliseconds(10),
            CancellationToken.None);

        using (Assert.Multiple())
        {
            await Assert.That(result.TimedOut).IsTrue();
            await Assert.That(result.WasCancellationTokenRespected).IsTrue();
        }
    }

    [Test]
    public async Task Fault_Completed_Before_Deadline_Is_Not_Claimed_By_Timeout()
    {
        var exception = await Assert.ThrowsAsync<TimeoutException>(async () =>
            await TimeoutHelper.ExecuteWithTimeoutAndDetailsAsync(
                _ => Task.FromException<bool>(new TimeoutException("Inner operation timed out.")),
                TimeSpan.FromMilliseconds(10),
                CancellationToken.None));

        await Assert.That(exception!.Message).IsEqualTo("Inner operation timed out.");
    }

    [Test]
    public async Task Completion_With_Asynchronous_Continuations_Before_Deadline_Is_Not_Claimed_By_Timeout()
    {
        var completion = new TaskCompletionSource<bool>(
            TaskCreationOptions.RunContinuationsAsynchronously);

        var execution = TimeoutHelper.ExecuteWithTimeoutAndDetailsAsync(
            _ => completion.Task,
            TimeSpan.FromSeconds(1),
            CancellationToken.None);

        completion.SetResult(true);

        var result = await execution;

        using (Assert.Multiple())
        {
            await Assert.That(result.TimedOut).IsFalse();
            await Assert.That(result.Value).IsTrue();
        }
    }

    [Test]
    public async Task Completed_Execution_Published_After_Deadline_Signal_Wins()
    {
        using var attemptCancellation = new CancellationTokenSource();
        var cancellationSignals = new TimeoutHelper.CancellationSignals<bool>(attemptCancellation);
        var signalState = cancellationSignals.Deadline;

        signalState.SignalCancellation();
        cancellationSignals.PublishExecutionTask(Task.FromResult(true));

        using (Assert.Multiple())
        {
            await Assert.That(await signalState.Signal.Task).IsTrue();
            await Assert.That(attemptCancellation.IsCancellationRequested).IsTrue();
        }
    }

    [Test]
    public async Task Cancelled_Execution_Published_After_Deadline_Signal_Belongs_To_Deadline()
    {
        using var attemptCancellation = new CancellationTokenSource();
        var cancellationSignals = new TimeoutHelper.CancellationSignals<bool>(attemptCancellation);
        var signalState = cancellationSignals.Deadline;

        signalState.SignalCancellation();
        cancellationSignals.PublishExecutionTask(Task.FromCanceled<bool>(attemptCancellation.Token));

        await Assert.That(await signalState.Signal.Task).IsFalse();
    }

    [Test]
    public async Task Execution_Publication_Is_Shared_By_Both_Cancellation_Signals()
    {
        using var attemptCancellation = new CancellationTokenSource();
        var cancellationSignals = new TimeoutHelper.CancellationSignals<bool>(attemptCancellation);

        cancellationSignals.PublishExecutionTask(Task.FromResult(true));
        cancellationSignals.Deadline.SignalCancellation();
        cancellationSignals.ExternalCancellation.SignalCancellation();

        using (Assert.Multiple())
        {
            await Assert.That(await cancellationSignals.Deadline.Signal.Task).IsTrue();
            await Assert.That(await cancellationSignals.ExternalCancellation.Signal.Task).IsTrue();
        }
    }

    [Test]
    public async Task Timeout_Claims_Tokenless_Cooperative_Cancellation()
    {
        var result = await TimeoutHelper.ExecuteWithTimeoutAndDetailsAsync(
            timeoutToken =>
            {
                timeoutToken.WaitHandle.WaitOne(TimeSpan.FromSeconds(1));
                var completion = new TaskCompletionSource<bool>(
                    TaskCreationOptions.RunContinuationsAsynchronously);
                completion.TrySetCanceled();
                return completion.Task;
            },
            TimeSpan.FromMilliseconds(10),
            CancellationToken.None);

        using (Assert.Multiple())
        {
            await Assert.That(result.TimedOut).IsTrue();
            await Assert.That(result.WasCancellationTokenRespected).IsTrue();
        }
    }
}
