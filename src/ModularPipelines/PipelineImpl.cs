using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Initialization.Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ModularPipelines.Engine;
using ModularPipelines.Engine.Dependencies;
using ModularPipelines.Engine.Executors;
using ModularPipelines.Exceptions;
using ModularPipelines.Helpers;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.PipelineCli;

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
    public async Task<PipelineSummary> RunAsync(CancellationToken cancellationToken = default)
    {
        if (await Services.GetRequiredService<PipelineCommandHandler>()
                .TryExecuteAsync(cancellationToken)
                .ConfigureAwait(false) is { } commandResult)
        {
            return commandResult;
        }

        return await Services.GetRequiredService<IExecutionOrchestrator>()
            .ExecuteAsync(cancellationToken)
            .ConfigureAwait(false);
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
