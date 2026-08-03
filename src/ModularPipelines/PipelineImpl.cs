using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
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
    private readonly IHost _host;
    private readonly AsyncServiceScope _serviceScope;

    [ExcludeFromCodeCoverage]
    ~PipelineImpl()
    {
        DisposeAsync().AsTask().GetAwaiter().GetResult();
    }

    private PipelineImpl(IHost host)
    {
        _host = host;
        _serviceScope = host.Services.CreateAsyncScope();

        Disposer.RegisterOnShutdown(this);
    }

    internal static async Task<PipelineImpl> CreateAsync(IHostBuilder hostBuilder)
    {
        var host = new PipelineImpl(hostBuilder.Build());
        var services = host._host.Services;

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
            return host;
        }

        await services.InitializeAsync().ConfigureAwait(false);
        return host;
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
            if (await Services.GetRequiredService<PipelineCommandHandler>()
                    .TryExecuteAsync(cancellationToken)
                    .ConfigureAwait(false) is { } commandResult)
            {
                summary = commandResult;
            }
            else if (Services.GetRequiredService<IOptions<PipelineOptions>>().Value.DryRun)
            {
                var plan = await PlanAsync(cancellationToken).ConfigureAwait(false);
                Services.GetRequiredService<PipelinePlanPrinter>().Print(plan);
                var now = DateTimeOffset.UtcNow;
                summary = new PipelineSummary(plan.Modules, [], TimeSpan.Zero, now, now);
            }
            else
            {
                summary = await Services.GetRequiredService<IExecutionOrchestrator>()
                    .ExecuteAsync(cancellationToken)
                    .ConfigureAwait(false);
            }

            ModuleActivityTracing.RecordPipelineCompletion(
                activity,
                summary.Status.ToString(),
                summary.Status == Status.Failed);
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
    public async ValueTask DisposeAsync()
    {
        await _serviceScope.DisposeAsync().ConfigureAwait(false);
        await Disposer.DisposeObjectAsync(_host).ConfigureAwait(false);
        GC.SuppressFinalize(this);
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
