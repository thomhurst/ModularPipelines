using System.Diagnostics;
using System.Runtime.ExceptionServices;
using Initialization.Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using ModularPipelines.Engine;
using ModularPipelines.Engine.Dependencies;
using ModularPipelines.Engine.Executors;
using ModularPipelines.Enums;
using ModularPipelines.Exceptions;
using ModularPipelines.Helpers;
using ModularPipelines.Interfaces;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using ModularPipelines.PipelineCli;
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
    private readonly object _disposeLock = new();
    private readonly AsyncLocal<DisposalOwnership?> _disposalOwnership = new();
    private int _isSynchronousContainerDisposalActive;
    private Task? _disposeTask;

    private PipelineImpl(IHost host, PipelineBuilderResources builderResources)
    {
        _host = host;
        _builderResources = builderResources;
        _serviceScope = host.Services.CreateAsyncScope();

        _shutdownRegistration = Disposer.RegisterOnShutdownWithUnregistration(this);
    }

    internal static async Task<PipelineImpl> CreateAsync(
        IHostBuilder hostBuilder,
        PipelineBuilderResources builderResources,
        bool initializePipeline)
    {
        var pipeline = new PipelineImpl(hostBuilder.Build(), builderResources);
        var services = pipeline._host.Services;

        try
        {
            if (!initializePipeline)
            {
                return pipeline;
            }

            try
            {
                ValidateModuleDependencies(services, services.GetServices<IModule>());
            }
            catch (Exception exception) when (exception is ModuleNotRegisteredException
                or ModuleSelfDependencyException
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
                    var plan = await PlanAsync(cancellationToken).ConfigureAwait(false);
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
                summary.Status.ToString(),
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

    /// <inheritdoc />
    public ValueTask DisposeAsync()
    {
        TaskCompletionSource completion;
        Task disposeTask;
        lock (_disposeLock)
        {
            if (_disposeTask is not null)
            {
                return _disposalOwnership.Value?.IsActive == true
                    || Volatile.Read(ref _isSynchronousContainerDisposalActive) == 1
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
