using System.Diagnostics;
using System.Reflection;
using Initialization.Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ModularPipelines.Extensions;
using ModularPipelines.Helpers;

namespace ModularPipelines.Host;

/// <summary>
/// The host for executing ModularPipelines.
/// </summary>
internal class PipelineHost : IPipelineHost
{
    private readonly IHost _hostImplementation;
    private readonly AsyncServiceScope _serviceScope;

    private PipelineHost(IHost hostImplementation)
    {
        _hostImplementation = hostImplementation;
        _serviceScope = hostImplementation.Services.CreateAsyncScope();

        Disposer.RegisterOnShutdown(this);
    }

    public static async Task<PipelineHost> Create(IHostBuilder hostBuilder)
    {
        var host = new PipelineHost(hostBuilder.Build());

        await host.RootServices.InitializeAsync();

        return host;
    }

    ~PipelineHost()
    {
        Dispose();
    }

    [StackTraceHidden]
    public Task StartAsync(CancellationToken cancellationToken = default)
    {
        return this.ExecutePipelineAsync();
    }

    [StackTraceHidden]
    public Task StopAsync(CancellationToken cancellationToken = default)
    {
        return _hostImplementation.StopAsync(cancellationToken);
    }

    public IServiceProvider Services
    {
        [StackTraceHidden]
        get => _serviceScope.ServiceProvider;
    }

    public void Dispose()
    {
        DisposeAsync().AsTask().GetAwaiter().GetResult();
        GC.SuppressFinalize(this);
    }

    public async ValueTask DisposeAsync()
    {
        var disposables = Services.GetType()
            .GetProperty("Disposables", BindingFlags.Instance | BindingFlags.NonPublic)
            ?.GetValue(Services) as List<object>;

        foreach (var disposable in disposables?.OfType<IDisposable>() ?? Array.Empty<IDisposable>())
        {
            await Disposer.DisposeObjectAsync(disposable);
        }

        await Disposer.DisposeObjectAsync(_hostImplementation);
        GC.SuppressFinalize(this);
    }

    public IServiceProvider RootServices => _hostImplementation.Services;
}