using System.Diagnostics;
using System.Runtime.ExceptionServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using ModularPipelines.Console;
using ModularPipelines.DependencyInjection;
using ModularPipelines.Distributed;
using ModularPipelines.Engine;
using ModularPipelines.Engine.Dependencies;
using ModularPipelines.Engine.Executors;
using ModularPipelines.Enums;
using ModularPipelines.Exceptions;
using ModularPipelines.Helpers;
using ModularPipelines.Interfaces;
using ModularPipelines.Logging;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using ModularPipelines.PipelineCli;
using ModularPipelines.Reporting;
using ModularPipelines.Secrets;
using ModularPipelines.Tracing;

namespace ModularPipelines;

/// <summary>
/// Internal implementation of the pipeline.
/// </summary>
internal sealed class PipelineImpl : IPipeline
{
    private const string DisposalExceptionDataKey = "ModularPipelines.PipelineDisposalException";

    private readonly IHost _host;
    private readonly PipelineBuilderResources _builderResources;
    private readonly AsyncServiceScope _serviceScope;
    private readonly IDisposable _shutdownRegistration;
    private readonly Lock _disposeLock = new();
    private readonly AsyncLocal<DisposalOwnership?> _disposalOwnership = new();
    private IConsoleCoordinator? _consoleCoordinator;
    private int _isSynchronousContainerDisposalActive;
    private Task? _disposeTask;
    private TaskCompletionSource? _runCompletion;
    private int _activeRuns;
    private bool _shutdownRequested;

    private PipelineImpl(IHost host, PipelineBuilderResources builderResources)
    {
        _host = host;
        _builderResources = builderResources;
        _serviceScope = host.Services.CreateAsyncScope();

        _shutdownRegistration = Disposer.RegisterOnShutdownWithUnregistration(ShutdownAsync);
    }

    internal static async Task<PipelineImpl> CreateAsync(
        IHostBuilder hostBuilder,
        PipelineBuilderResources builderResources,
        bool initializePipeline,
        Func<IServiceProvider, CancellationToken, Task>? beforeInitialization = null,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var pipeline = new PipelineImpl(hostBuilder.Build(), builderResources);
        var services = pipeline._host.Services;

        try
        {
            cancellationToken.ThrowIfCancellationRequested();
            services.GetRequiredService<EngineCancellationToken>().IsRunActive =
                () => Volatile.Read(ref pipeline._activeRuns) > 0;

            var consoleCoordinator = services.GetRequiredService<IConsoleCoordinator>();
            pipeline._consoleCoordinator = consoleCoordinator;
            consoleCoordinator.Install();
            consoleCoordinator.EnableOutputBuffering();
            cancellationToken.ThrowIfCancellationRequested();

            if (!initializePipeline)
            {
                return pipeline;
            }

            if (beforeInitialization is not null)
            {
                await beforeInitialization(pipeline.Services, cancellationToken).ConfigureAwait(false);
            }

            cancellationToken.ThrowIfCancellationRequested();
            await ValidateOptionsOnStartAsync(services, cancellationToken).ConfigureAwait(false);
            cancellationToken.ThrowIfCancellationRequested();
            var modules = services.GetServices<IModule>().ToArray();
            ValidateModuleIds(modules);

            try
            {
                ValidateModuleDependencies(services, modules);
            }
            catch (Exception exception) when (exception is ModuleNotRegisteredException
                or ModuleSelfDependencyException
                or DependencyCollisionException)
            {
                await PipelineServiceInitializer.InitializeAsync(services, cancellationToken).ConfigureAwait(false);
                var runnableModules = await services.GetRequiredService<ModuleRetriever>()
                    .GetRunnableModulesForValidation(cancellationToken)
                    .ConfigureAwait(false);
                cancellationToken.ThrowIfCancellationRequested();
                ValidateModuleDependencies(services, runnableModules);
                return pipeline;
            }

            await PipelineServiceInitializer.InitializeAsync(services, cancellationToken).ConfigureAwait(false);
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
        lock (_disposeLock)
        {
            ObjectDisposedException.ThrowIf(_shutdownRequested || _disposeTask is not null, this);
            if (_activeRuns++ == 0)
            {
                _runCompletion = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
            }
        }

        try
        {
            return await RunCoreAsync(cancellationToken).ConfigureAwait(false);
        }
        finally
        {
            lock (_disposeLock)
            {
                if (--_activeRuns == 0)
                {
                    _runCompletion!.TrySetResult();
                }
            }
        }
    }

    private async Task<PipelineSummary> RunCoreAsync(CancellationToken cancellationToken)
    {
        var pipelineName = Services.GetRequiredService<IHostEnvironment>().ApplicationName;
        using var activity = ModuleActivityTracing.StartPipelineActivity(pipelineName);

        try
        {
            PipelineSummary summary;
            // Help must bypass PipelineCommandHandler because constructing it enumerates and activates modules.
            if (Services.GetRequiredService<PipelineCommandLineOptions>().Command == PipelineCommand.Help)
            {
                summary = PipelineCommandLineHelp.Show(Services.GetRequiredService<IConsoleWriter>());
            }
            else if (await Services.GetRequiredService<PipelineCommandHandler>()
                    .TryExecuteAsync(cancellationToken)
                    .ConfigureAwait(false) is { } commandResult)
            {
                summary = commandResult;
            }
            else
            {
                Services.GetRequiredService<PipelineExecutionState>().MarkExecutionStarted();
                if (Services.GetRequiredService<IOptions<PipelineOptions>>().Value.DryRun)
                {
                    using var planningCancellation = CancellationTokenSource.CreateLinkedTokenSource(
                        cancellationToken,
                        Services.GetRequiredService<EngineCancellationToken>().Token);
                    var plan = await PlanAsync(planningCancellation.Token).ConfigureAwait(false);
                    Services.GetRequiredService<PipelinePlanPrinter>().Print(plan);
                    var now = DateTimeOffset.UtcNow;
                    summary = new PipelineSummary(plan.Modules, [], TimeSpan.Zero, now, now)
                    {
                        StatusOverride = ModuleStatus.Succeeded,
                    };
                }
                else
                {
                    summary = await Services.GetRequiredService<IExecutionOrchestrator>()
                        .ExecuteAsync(cancellationToken)
                        .ConfigureAwait(false);
                }
            }

            ModuleActivityTracing.RecordPipelineCompletion(
                activity,
                summary.Status,
                summary.Status == ModuleStatus.Failed);
            return summary;
        }
        catch (Exception exception)
        {
            var secretObfuscator = Services.GetRequiredService<ISecretObfuscator>();
            ModuleActivityTracing.RecordPipelineFailure(
                activity,
                exception,
                secretObfuscator.Obfuscate(exception.Message, null));
            var maskedException = ObfuscatedLogException.CreateIfNeeded(exception, secretObfuscator);
            if (!ReferenceEquals(maskedException, exception))
            {
                throw maskedException;
            }

            throw;
        }
    }

    /// <inheritdoc />
    public Task ExportDependencyGraphAsync(
        DependencyGraphFormat format,
        string path,
        CancellationToken cancellationToken = default) =>
        Services.GetRequiredService<IDependencyGraphExporter>()
            .ExportAsync(format, path, cancellationToken);

    private async Task ShutdownAsync()
    {
        Task runCompletion;
        lock (_disposeLock)
        {
            _shutdownRequested = true;
            runCompletion = _runCompletion?.Task ?? Task.CompletedTask;
        }

        try
        {
            _host.Services.GetRequiredService<EngineCancellationToken>().RequestShutdown();
        }
        finally
        {
            // Keep services alive until cleanup modules, end handlers, reports and tracing finish.
            // The process-exit registration bounds this entire operation.
            await runCompletion.ConfigureAwait(false);
            // Shutdown is an external waiter, even while another thread is synchronously
            // disposing the container. It must not take the reentrant-disposal shortcut.
            await DisposeAsync(allowReentrancy: false).ConfigureAwait(false);
        }
    }

    /// <inheritdoc />
    public ValueTask DisposeAsync() => DisposeAsync(allowReentrancy: true);

    private ValueTask DisposeAsync(bool allowReentrancy)
    {
        TaskCompletionSource completion;
        Task disposeTask;
        lock (_disposeLock)
        {
            if (_disposeTask is not null)
            {
                return allowReentrancy && (_disposalOwnership.Value?.IsActive == true
                    || Volatile.Read(ref _isSynchronousContainerDisposalActive) == 1)
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
            var disposalExceptions = new List<Exception>();
            try
            {
                ValueTask scopeDisposal;
                Volatile.Write(ref _isSynchronousContainerDisposalActive, 1);
                try
                {
                    scopeDisposal = _serviceScope.DisposeAsync();
                }
                finally
                {
                    Volatile.Write(ref _isSynchronousContainerDisposalActive, 0);
                }

                await scopeDisposal.ConfigureAwait(false);
            }
            catch (Exception exception)
            {
                disposalExceptions.Add(exception);
            }

            try
            {
                Task hostDisposal;
                Volatile.Write(ref _isSynchronousContainerDisposalActive, 1);
                try
                {
                    hostDisposal = Disposer.DisposeObjectAsync(_host);
                }
                finally
                {
                    Volatile.Write(ref _isSynchronousContainerDisposalActive, 0);
                }

                await hostDisposal.ConfigureAwait(false);
            }
            catch (Exception exception)
            {
                disposalExceptions.Add(exception);
            }

            try
            {
                // A throwing service can stop container disposal before it reaches the
                // console coordinator. Always release our process-wide installation.
                _consoleCoordinator?.Uninstall();
            }
            catch (Exception exception)
            {
                disposalExceptions.Add(exception);
            }

            try
            {
                _builderResources.Dispose();
            }
            catch (Exception exception)
            {
                disposalExceptions.Add(exception);
            }

            if (disposalExceptions.Count == 1)
            {
                ExceptionDispatchInfo.Capture(disposalExceptions[0]).Throw();
            }

            if (disposalExceptions.Count > 1)
            {
                throw new AggregateException(disposalExceptions);
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

    /// <summary>
    /// Runs startup options validation with the same validator selection as the .NET 11 host:
    /// a sync-only <see cref="IStartupValidator"/> replacement takes precedence for compatibility;
    /// otherwise every <see cref="IAsyncStartupValidator"/> runs and failures are aggregated.
    /// </summary>
    private static async Task ValidateOptionsOnStartAsync(IServiceProvider services, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
#pragma warning disable SYSLIB0066 // IStartupValidator is obsolete but consumers may still replace it.
        var startupValidator = services.GetService<IStartupValidator>();
#pragma warning restore SYSLIB0066
        var asyncValidators = startupValidator is not null and not IAsyncStartupValidator
            ? []
            : services.GetServices<IAsyncStartupValidator>().Distinct(ReferenceEqualityComparer.Instance).Cast<IAsyncStartupValidator>().ToArray();

        if (startupValidator is not null
            && !asyncValidators.Any(asyncValidator => ReferenceEquals(asyncValidator, startupValidator)))
        {
            cancellationToken.ThrowIfCancellationRequested();
            startupValidator.Validate();
            return;
        }

        List<Exception> failures = [];
        foreach (var asyncValidator in asyncValidators)
        {
            cancellationToken.ThrowIfCancellationRequested();
            try
            {
                await asyncValidator.ValidateAsync(cancellationToken).ConfigureAwait(false);
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (OptionsValidationException exception)
            {
                failures.Add(exception);
            }
            catch (AggregateException exception)
            {
                failures.AddRange(exception.InnerExceptions);
            }
            catch (Exception exception)
            {
                // Like the host, keep earlier validation failures and stop at an unexpected fault.
                failures.Add(exception);
                break;
            }
        }

        if (failures.Count == 1)
        {
            ExceptionDispatchInfo.Capture(failures[0]).Throw();
        }

        if (failures.Count > 1)
        {
            throw new AggregateException(failures);
        }
    }

    private static void ValidateModuleIds(IEnumerable<IModule> modules)
    {
        var moduleTypes = new Dictionary<ModuleId, Type>();
        foreach (var module in modules)
        {
            var moduleType = module.GetType();
            var moduleId = ModuleId.FromType(moduleType);
            if (moduleTypes.TryGetValue(moduleId, out var existingType) && existingType != moduleType)
            {
                throw new InvalidOperationException(
                    $"Module identifier '{moduleId}' is already registered for '{existingType}'. " +
                    $"It cannot also identify '{moduleType}'.");
            }

            moduleTypes[moduleId] = moduleType;
        }
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
