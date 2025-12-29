using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
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

    [ExcludeFromCodeCoverage]
    ~PipelineHost()
    {
        Dispose();
    }

    private PipelineHost(IHost hostImplementation)
    {
        _hostImplementation = hostImplementation;
        _serviceScope = hostImplementation.Services.CreateAsyncScope();

        Disposer.RegisterOnShutdown(this);
    }

    public static async Task<PipelineHost> Create(IHostBuilder hostBuilder)
    {
        var host = new PipelineHost(hostBuilder.Build());

        await host.RootServices.InitializeAsync().ConfigureAwait(false);

        return host;
    }

    [ExcludeFromCodeCoverage]
    [StackTraceHidden]
    public Task StartAsync(CancellationToken cancellationToken = default)
    {
        return this.ExecutePipelineAsync();
    }

    [ExcludeFromCodeCoverage]
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

    [ExcludeFromCodeCoverage]
    public void Dispose()
    {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
#pragma warning disable CA2012
        DisposeAsync();
#pragma warning restore CA2012
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
    }

    public async ValueTask DisposeAsync()
    {
        var disposables = Services.GetType()
            .GetProperty("Disposables", BindingFlags.Instance | BindingFlags.NonPublic)
            ?.GetValue(Services) as List<object>;

        foreach (var disposable in disposables?.OfType<IDisposable>() ?? Array.Empty<IDisposable>())
        {
            await Disposer.DisposeObjectAsync(disposable).ConfigureAwait(false);
        }

        await Disposer.DisposeObjectAsync(_hostImplementation).ConfigureAwait(false);
        GC.SuppressFinalize(this);
    }

    public IServiceProvider RootServices => _hostImplementation.Services;
}