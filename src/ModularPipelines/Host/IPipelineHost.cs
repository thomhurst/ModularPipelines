using Microsoft.Extensions.Hosting;
using ModularPipelines.Models;

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

    /// <summary>
    /// Executes the pipeline and returns a summary of results.
    /// </summary>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <returns>A summary of the pipeline execution results.</returns>
    Task<PipelineSummary> RunAsync(CancellationToken cancellationToken = default);
}
