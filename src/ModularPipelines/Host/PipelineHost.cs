using System.Diagnostics;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ModularPipelines.Helpers;

namespace ModularPipelines.Host;

internal class PipelineHost : IPipelineHost
{
    private readonly IHost _hostImplementation;
    private readonly AsyncServiceScope _serviceScope;

    public PipelineHost(IHost hostImplementation)
    {
        _hostImplementation = hostImplementation;
        _serviceScope = hostImplementation.Services.CreateAsyncScope();

        Disposer.RegisterOnShutdown(this);
    }
    
    ~PipelineHost()
    {
        Dispose();
    }

    [StackTraceHidden]
    public Task StartAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        return _hostImplementation.StartAsync(cancellationToken);
    }

    [StackTraceHidden]
    public Task StopAsync(CancellationToken cancellationToken = new CancellationToken())
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