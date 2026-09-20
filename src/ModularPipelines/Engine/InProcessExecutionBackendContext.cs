using ModularPipelines.Engine.Execution;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine;

internal sealed class InProcessExecutionBackendContext : IExecutionBackendContext, IAsyncDisposable
{
    private readonly IExecutionBackendContext _resultContext;
    private readonly IModuleRunner _moduleRunner;
    private readonly SemaphoreSlim _executionLimit;
    private readonly EngineCancellationToken _engineCancellationToken;
    private readonly Dictionary<Type, IModule> _modules;
    private readonly Lazy<Task<IModuleScheduler>> _scheduler;
    private readonly Dictionary<IModule, Lazy<Task<IModuleResult>>> _executions = new(ReferenceEqualityComparer.Instance);
    private readonly CancellationTokenSource _lifetime = new();
    private readonly Lock _sync = new();
    private IModuleScheduler? _initializedScheduler;
    private TaskCompletionSource _completionChanged = new(TaskCreationOptions.RunContinuationsAsynchronously);
    private bool _disposed;

    public InProcessExecutionBackendContext(
        IExecutionBackendContext resultContext,
        IModuleRunner moduleRunner,
        IReadOnlyList<IModule> modules,
        Func<Task<IModuleScheduler>> initializeScheduler,
        int maxParallelism,
        EngineCancellationToken engineCancellationToken)
    {
        _resultContext = resultContext;
        _moduleRunner = moduleRunner;
        _executionLimit = new SemaphoreSlim(maxParallelism, maxParallelism);
        _engineCancellationToken = engineCancellationToken;
        _modules = modules.ToDictionary(module => module.GetType());
        _scheduler = new Lazy<Task<IModuleScheduler>>(async () =>
        {
            var scheduler = await initializeScheduler().ConfigureAwait(false);
            try
            {
                lock (_sync)
                {
                    // Publish only after replay succeeds. Concurrent remote results either
                    // participate in this replay or see the fully initialized scheduler.
                    foreach (var module in modules)
                    {
                        if (module is IInternalModule internalModule && internalModule.ResultTask is { IsCompletedSuccessfully: true } resultTask)
                        {
                            ApplySchedulerResult(scheduler, module, resultTask.Result);
                        }
                    }

                    _initializedScheduler = scheduler;
                }

                return scheduler;
            }
            catch
            {
                scheduler.Dispose();
                throw;
            }
        });
    }

    public Task<IModuleResult> ExecuteModuleAsync(IModule module, CancellationToken cancellationToken = default)
    {
        ValidateModule(module);
        var requestToken = GetRequestCancellationToken(module, cancellationToken);
        if (requestToken.IsCancellationRequested)
        {
            throw new OperationCanceledException(cancellationToken);
        }

        Lazy<Task<IModuleResult>> execution;
        var created = false;
        lock (_sync)
        {
            ObjectDisposedException.ThrowIf(_disposed, this);
            if (!_executions.TryGetValue(module, out execution!))
            {
                execution = new Lazy<Task<IModuleResult>>(() => ExecuteCoreAsync(module, cancellationToken));
                _executions.Add(module, execution);
                created = true;
            }
        }

        return created ? execution.Value : WaitForExecutionAsync(execution.Value, requestToken, cancellationToken);
    }

    public bool TryApplyResult(IModule module, IModuleResult result)
    {
        ValidateModule(module);
        bool applied;
        IModuleScheduler? scheduler;
        lock (_sync)
        {
            ObjectDisposedException.ThrowIf(_disposed, this);
            if (_executions.TryGetValue(module, out var execution)
                && (!execution.IsValueCreated || !execution.Value.IsCompleted))
            {
                throw new InvalidOperationException("Cannot apply a remote result while the module is executing locally.");
            }

            applied = _resultContext.TryApplyResult(module, result);
            scheduler = _initializedScheduler;
        }

        if (scheduler is not null && module.AsInternal().ResultTask is { IsCompletedSuccessfully: true } resultTask)
        {
            ApplySchedulerResult(scheduler, module, resultTask.Result);
            SignalCompletion();
        }

        return applied;
    }

    private CancellationToken GetRequestCancellationToken(IModule module, CancellationToken cancellationToken) =>
        module.Configuration.AlwaysRun && cancellationToken == _engineCancellationToken.Token
            ? _engineCancellationToken.NonFailureCancellationToken
            : cancellationToken;

    private static async Task<IModuleResult> WaitForExecutionAsync(
        Task<IModuleResult> execution, CancellationToken requestToken, CancellationToken callerToken)
    {
        try
        {
            return await execution.WaitAsync(requestToken).ConfigureAwait(false);
        }
        catch (OperationCanceledException exception) when (
            WorkerCancellationClassifier.IsExpected(exception, requestToken))
        {
            throw new NormalizedWorkerCancellationException(exception.Message, exception, callerToken);
        }
    }

    private async Task<IModuleResult> ExecuteCoreAsync(IModule module, CancellationToken cancellationToken)
    {
        using var linkedCancellation = CancellationTokenSource.CreateLinkedTokenSource(
            GetRequestCancellationToken(module, cancellationToken), _lifetime.Token);
        var token = linkedCancellation.Token;
        try
        {
            var scheduler = await _scheduler.Value.ConfigureAwait(false);
            var state = scheduler.GetModuleState(module.GetType())!;
            var resultTask = module.AsInternal().ResultTask;
            while (state.State != ModuleExecutionState.Completed && !resultTask.IsCompleted)
            {
                Task completionChanged;
                lock (_sync)
                {
                    completionChanged = _completionChanged.Task;
                }

                await _moduleRunner.ExecuteAsync(state, _executionLimit, token).ConfigureAwait(false);
                if (state.ExecutionDeferred)
                {
                    // A competing module can acquire a constraint between scheduling and
                    // execution. Retry after a completion, without spinning or losing a wakeup.
                    await completionChanged.WaitAsync(token).ConfigureAwait(false);
                }
                else
                {
                    break;
                }
            }

            return await resultTask.ConfigureAwait(false);
        }
        catch (OperationCanceledException exception) when (
            WorkerCancellationClassifier.IsExpected(exception, token))
        {
            throw new NormalizedWorkerCancellationException(exception.Message, exception, cancellationToken);
        }
        finally
        {
            SignalCompletion();
        }
    }

    private static void ApplySchedulerResult(IModuleScheduler scheduler, IModule module, IModuleResult result)
    {
        if (scheduler.GetModuleState(module.GetType()) is { } state)
        {
            state.Result = result;
        }

        var success = result.Status is ModuleStatus.Succeeded or ModuleStatus.FailureIgnored
            or ModuleStatus.Skipped or ModuleStatus.RestoredFromHistory or ModuleStatus.RestoredFromCache;
        scheduler.MarkModuleCompleted(module.GetType(), success,
            result.ExceptionOrDefault, result.Status);
    }

    private void ValidateModule(IModule module)
    {
        ArgumentNullException.ThrowIfNull(module);
        if (!_modules.TryGetValue(module.GetType(), out var plannedModule) || !ReferenceEquals(module, plannedModule))
        {
            throw new ArgumentException("The module must be the instance supplied in this execution plan.", nameof(module));
        }
    }

    private void SignalCompletion()
    {
        TaskCompletionSource previous;
        lock (_sync)
        {
            previous = _completionChanged;
            _completionChanged = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        }

        previous.TrySetResult();
    }

    public async ValueTask DisposeAsync()
    {
        Lazy<Task<IModuleResult>>[] executions;
        lock (_sync)
        {
            if (_disposed)
            {
                return;
            }

            _disposed = true;
            executions = _executions.Values.ToArray();
        }

        await _lifetime.CancelAsync().ConfigureAwait(false);
        Task completed = Task.WhenAll(executions.Select(execution => execution.Value));
        // Execution failures are observed by the backend and engine error handling. Cleanup
        // must drain requests without replacing the exception which caused backend teardown.
        await completed.ConfigureAwait(ConfigureAwaitOptions.SuppressThrowing);
        if (_scheduler.IsValueCreated && _scheduler.Value.IsCompletedSuccessfully)
        {
            _scheduler.Value.Result.Dispose();
        }

        _executionLimit.Dispose();
        _lifetime.Dispose();
    }
}
