using Microsoft.Extensions.Hosting;

namespace ModularPipelines.Host;

/// <summary>
/// The host responsible for executing the pipeline.
/// </summary>
public interface IPipelineHost : IHost, IAsyncDisposable
{
    /// <summary>
    /// Gets the pipeline's service provider.
    /// </summary>
    internal IServiceProvider RootServices { get; }
}