using System.Runtime.CompilerServices;
using Microsoft.Extensions.Hosting;

namespace ModularPipelines.Host;

internal class PipelineHost : IPipelineHost
{
    private readonly IHost _hostImplementation;

    public PipelineHost(IHost hostImplementation)
    {
        _hostImplementation = hostImplementation;
    }
    
    ~PipelineHost()
    {
        Dispose();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Dispose()
    {
        GC.SuppressFinalize(this);
        _hostImplementation.Dispose();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Task StartAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        return _hostImplementation.StartAsync(cancellationToken);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Task StopAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        return _hostImplementation.StopAsync(cancellationToken);
    }

    public IServiceProvider Services
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _hostImplementation.Services;
    }
}