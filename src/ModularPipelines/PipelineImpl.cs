using System.Diagnostics;
using System.Runtime.ExceptionServices;
using Initialization.Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using ModularPipelines.Engine;
using ModularPipelines.Engine.Dependencies;
using ModularPipelines.Engine.Executors;
using ModularPipelines.Exceptions;
using ModularPipelines.Helpers;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using ModularPipelines.PipelineCli;

namespace ModularPipelines;

/// <summary>
/// Internal implementation of the pipeline.
/// </summary>
internal sealed class PipelineImpl : IPipeline
{
    private const string DisposalExceptionDataKey = "ModularPipelines.PipelineDisposalException";

    private readonly IHost _host;
    private readonly AsyncServiceScope _serviceScope;
    private readonly IDisposable _shutdownRegistration;
    private readonly object _disposeLock = new();
    private readonly AsyncLocal<DisposalOwnership?> _disposalOwnership = new();
    private int _isContainerDisposalActive;
    private Task? _disposeTask;

    private PipelineImpl(IHost host)
    {
        _host = host;
        _serviceScope = host.Services.CreateAsyncScope();

        _shutdownRegistration = Disposer.RegisterOnShutdownWithUnregistration(this);
    }

    internal static async Task<PipelineImpl> CreateAsync(IHostBuilder hostBuilder)
    {
        var pipeline = new PipelineImpl(hostBuilder.Build());
        var services = pipeline._host.Services;

        try
        {
            try
            {
                ValidateModuleDependencies(services, services.GetServices<IModule>());
            }
            catch (Exception exception) when (exception is ModuleNotRegisteredException
                or ModuleReferencingSelfException
                or DependencyCollisionException)
            {
                await services.InitializeAsync().ConfigureAwait(false);
                var runnableModules = await services.GetRequiredService<ModuleRetriever>()
                    .GetRunnableModulesForValidation()
                    .ConfigureAwait(false);
                ValidateModuleDependencies(services, runnableModules);
                return pipeline;
            }

            await services.InitializeAsync().ConfigureAwait(false);
            return pipeline;
        }
        catch (Exception startupException)
        {
            try
            {
                await pipeline.DisposeAsync().ConfigureAwait(false);
            }
            catch (Exception disposalException)
            {
                startupException.Data[DisposalExceptionDataKey] = disposalException;
            }

            throw;
        }
    }

    /// <inheritdoc />
    public IServiceProvider Services
    {
        [StackTraceHidden]
        get => _serviceScope.ServiceProvider;
    }

    /// <inheritdoc />
    public Task<PipelinePlan> PlanAsync(CancellationToken cancellationToken = default) =>
        Services.GetRequiredService<PipelinePlanner>().CreateAsync(cancellationToken);

    /// <inheritdoc />
    public async Task<PipelineSummary> RunAsync(CancellationToken cancellationToken = default)
    {
        if (await Services.GetRequiredService<PipelineCommandHandler>()
                .TryExecuteAsync(cancellationToken)
                .ConfigureAwait(false) is { } commandResult)
        {
            return commandResult;
        }

        if (Services.GetRequiredService<IOptions<PipelineOptions>>().Value.DryRun)
        {
            var plan = await PlanAsync(cancellationToken).ConfigureAwait(false);
            Services.GetRequiredService<PipelinePlanPrinter>().Print(plan);
            var now = DateTimeOffset.UtcNow;
            return new PipelineSummary(plan.Modules, [], TimeSpan.Zero, now, now);
        }

        return await Services.GetRequiredService<IExecutionOrchestrator>()
            .ExecuteAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc />
    public ValueTask DisposeAsync()
    {
        TaskCompletionSource completion;
        Task disposeTask;
        lock (_disposeLock)
        {
            if (_disposeTask is not null)
            {
                // Container teardown can invoke user disposers on a worker without a flowed
                // ExecutionContext. Such reentry must not await the disposal waiting for it.
                return _disposalOwnership.Value?.IsActive == true
                    || Volatile.Read(ref _isContainerDisposalActive) == 1
                    ? ValueTask.CompletedTask
                    : new ValueTask(_disposeTask);
            }

            completion = new TaskCompletionSource(
                TaskCreationOptions.RunContinuationsAsynchronously);
            _disposeTask = completion.Task;
            disposeTask = _disposeTask;
        }

        _ = CompleteDisposalAsync(completion);
        return new ValueTask(disposeTask);
    }

    private async Task CompleteDisposalAsync(TaskCompletionSource completion)
    {
        try
        {
            await DisposeCoreAsync().ConfigureAwait(false);
            completion.SetResult();
        }
        catch (OperationCanceledException cancellationException)
        {
            completion.SetCanceled(cancellationException.CancellationToken);
        }
        catch (Exception exception)
        {
            completion.SetException(exception);
        }
    }

    private async Task DisposeCoreAsync()
    {
        var ownership = new DisposalOwnership();
        _disposalOwnership.Value = ownership;
        try
        {
            _shutdownRegistration.Dispose();
            Volatile.Write(ref _isContainerDisposalActive, 1);
            try
            {
                Exception? scopeException = null;
                try
                {
                    await _serviceScope.DisposeAsync().ConfigureAwait(false);
                }
                catch (Exception exception)
                {
                    scopeException = exception;
                }

                try
                {
                    await Disposer.DisposeObjectAsync(_host).ConfigureAwait(false);
                }
                catch (Exception hostException) when (scopeException is not null)
                {
                    throw new AggregateException(scopeException, hostException);
                }

                if (scopeException is not null)
                {
                    ExceptionDispatchInfo.Capture(scopeException).Throw();
                }
            }
            finally
            {
                Volatile.Write(ref _isContainerDisposalActive, 0);
            }
        }
        finally
        {
            ownership.Deactivate();
            _disposalOwnership.Value = null;
        }
    }

    private sealed class DisposalOwnership
    {
        private int _isActive = 1;

        public bool IsActive => Volatile.Read(ref _isActive) == 1;

        public void Deactivate() => Interlocked.Exchange(ref _isActive, 0);
    }

    private static void ValidateModuleDependencies(
        IServiceProvider services,
        IEnumerable<IModule> modules)
    {
        ModuleDependencyValidator.Validate(
            modules,
            services.GetRequiredService<IModuleDependencyRegistry>(),
            services.GetRequiredService<IModuleMetadataRegistry>());
    }
}
