using System.Collections.Concurrent;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Initialization.Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ModularPipelines.Engine;
using ModularPipelines.Engine.Executors;
using ModularPipelines.Helpers;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines;

/// <summary>
/// Internal implementation of the pipeline.
/// </summary>
internal sealed class PipelineImpl : IPipeline
{
    private static readonly ConcurrentDictionary<Type, PropertyInfo?> DisposablesPropertyCache = new();

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

        // Validate module dependencies early
        ValidateModuleDependencies(host._host.Services);

        await host._host.Services.InitializeAsync().ConfigureAwait(false);

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
        return await Services.GetRequiredService<IExecutionOrchestrator>()
            .ExecuteAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async ValueTask DisposeAsync()
    {
        var servicesType = Services.GetType();
        var disposablesProperty = DisposablesPropertyCache.GetOrAdd(
            servicesType,
            static type => type.GetProperty("Disposables", BindingFlags.Instance | BindingFlags.NonPublic));

        var disposables = disposablesProperty?.GetValue(Services) as List<object>;

        foreach (var disposable in disposables?.OfType<IDisposable>() ?? Array.Empty<IDisposable>())
        {
            await Disposer.DisposeObjectAsync(disposable).ConfigureAwait(false);
        }

        await Disposer.DisposeObjectAsync(_host).ConfigureAwait(false);
        GC.SuppressFinalize(this);
    }

    private static void ValidateModuleDependencies(IServiceProvider services)
    {
        var modules = services.GetServices<IModule>();
        var moduleTypes = modules.Select(m => m.GetType());

        ModuleDependencyValidator.Validate(moduleTypes);
    }
}
