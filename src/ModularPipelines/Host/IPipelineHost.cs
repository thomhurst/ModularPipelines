using Microsoft.Extensions.Hosting;

namespace ModularPipelines.Host;

public interface IPipelineHost : IHost, IAsyncDisposable
{
    internal IServiceProvider RootServices { get; } 
}