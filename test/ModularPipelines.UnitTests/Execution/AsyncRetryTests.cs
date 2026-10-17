using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Engine;
using ModularPipelines.Enums;
using ModularPipelines.Extensions;
using ModularPipelines.Exceptions;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests.Execution;

public class AsyncRetryTests : TestBase
{
    private sealed class RetryState
    {
        public int RetryCount { get; init; } = 3;
        public int StopAfterAttempt { get; init; } = 2;
        public int SucceedOnAttempt { get; init; }
        public bool WaitForCancellation { get; init; }
        public bool ThrowFromPredicate { get; init; }
        public List<int> Attempts { get; } = [];
        public List<int> PredicateAttempts { get; } = [];
        public List<int> ContextAttempts { get; } = [];
        public List<Exception> PredicateExceptions { get; } = [];
        public Exception Failure { get; } = new InvalidOperationException("Module failure");
        public Exception PredicateFailure { get; } = new InvalidOperationException("Predicate failure");
        public TaskCompletionSource PredicateStarted { get; } = new(TaskCreationOptions.RunContinuationsAsynchronously);
        public CancellationToken PredicateToken { get; set; }
        public IModuleContext? Context { get; set; }
        public int BeforeAttempt { get; set; } = -1;
    }

    private sealed class RetryingModule(RetryState state) : Module<bool>
    {
        protected override void Configure(ModuleConfigurationBuilder module) => module
            .WithRetry(async (context, exception, attempt, token) =>
            {
                await Task.Yield();
                state.PredicateAttempts.Add(attempt);
                state.ContextAttempts.Add(context.Attempt);
                state.PredicateExceptions.Add(exception);
                state.PredicateToken = token;
                state.Context = context;
                state.PredicateStarted.TrySetResult();
                if (state.WaitForCancellation)
                {
                    await Task.Delay(Timeout.InfiniteTimeSpan, token);
                }

                if (state.ThrowFromPredicate)
                {
                    throw state.PredicateFailure;
                }

                return attempt < state.StopAfterAttempt;
            }, state.RetryCount, baseDelay: TimeSpan.Zero);

        protected override Task OnBeforeExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            state.BeforeAttempt = context.Attempt;
            return Task.CompletedTask;
        }

        protected internal override Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            state.Attempts.Add(context.Attempt);
            state.Context = context;
            if (context.Attempt == state.SucceedOnAttempt)
            {
                return Task.FromResult(true);
            }

            throw state.Failure;
        }
    }

    [Test]
    [Arguments(0, 10, 0, 1, 0)]
    [Arguments(3, 1, 0, 1, 1)]
    [Arguments(3, 2, 0, 2, 2)]
    [Arguments(2, 10, 0, 3, 2)]
    [Arguments(3, 10, 3, 3, 2)]
    [Arguments(3, 10, 1, 1, 0)]
    public async Task Predicate_Receives_Failed_Attempt_And_Stops_Retries(
        int retryCount, int stopAfterAttempt, int succeedOnAttempt, int executions, int predicates)
    {
        var state = new RetryState
        {
            RetryCount = retryCount,
            StopAfterAttempt = stopAfterAttempt,
            SucceedOnAttempt = succeedOnAttempt,
        };
        var builder = TestPipelineBuilder.Create().AddModule<RetryingModule>();
        builder.Services.AddSingleton(state);
        await using var host = await builder.BuildAsync();

        if (succeedOnAttempt > 0)
        {
            await host.RunAsync();
        }
        else
        {
            var exception = await Assert.ThrowsAsync<ModuleFailedException>(() => host.RunAsync());
            await Assert.That(exception!.InnerException).IsSameReferenceAs(state.Failure);
        }

        using (Assert.Multiple())
        {
            await Assert.That(state.Attempts).IsEquivalentTo(Enumerable.Range(1, executions));
            await Assert.That(state.PredicateAttempts).IsEquivalentTo(Enumerable.Range(1, predicates));
            await Assert.That(state.ContextAttempts).IsEquivalentTo(state.PredicateAttempts);
            await Assert.That(state.PredicateExceptions.All(e => ReferenceEquals(e, state.Failure))).IsTrue();
            await Assert.That(state.Context!.Attempt).IsEqualTo(executions);
            await Assert.That(state.BeforeAttempt).IsEqualTo(0);
            if (predicates > 0)
            {
                await Assert.That(state.PredicateToken.CanBeCanceled).IsTrue();
            }
        }
    }

    [Test]
    public async Task Predicate_Failure_Is_Not_Retried()
    {
        var state = new RetryState { ThrowFromPredicate = true };
        var builder = TestPipelineBuilder.Create().AddModule<RetryingModule>();
        builder.Services.AddSingleton(state);
        await using var host = await builder.BuildAsync();

        var exception = await Assert.ThrowsAsync<ModuleFailedException>(() => host.RunAsync());

        await Assert.That(exception!.InnerException).IsSameReferenceAs(state.PredicateFailure);
        await Assert.That(state.Attempts).IsEquivalentTo([1]);
        await Assert.That(state.PredicateAttempts).IsEquivalentTo([1]);
    }

    [Test]
    public async Task Cancellation_Interrupts_Async_Predicate_Without_Another_Attempt()
    {
        var state = new RetryState { WaitForCancellation = true };
        var builder = TestPipelineBuilder.Create().AddModule<RetryingModule>();
        builder.Services.AddSingleton(state);
        await using var host = await builder.BuildAsync();
        using var cancellation = new CancellationTokenSource();
        var run = host.RunAsync(cancellation.Token);
        await state.PredicateStarted.Task.WaitAsync(TimeSpan.FromSeconds(10));
        cancellation.Cancel();
        await run.WaitAsync(TimeSpan.FromSeconds(10));

        var result = host.Services.GetRequiredService<IModuleResultRegistry>().GetResult(typeof(RetryingModule));
        await Assert.That(result!.Status).IsEqualTo(ModuleStatus.Canceled);
        await Assert.That(state.Attempts).IsEquivalentTo([1]);
        await Assert.That(state.PredicateToken.IsCancellationRequested).IsTrue();
    }
}
