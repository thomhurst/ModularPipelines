using ModularPipelines.Models;

namespace ModularPipelines;

/// <summary>
/// Represents a built pipeline ready for execution.
/// </summary>
public interface IPipeline : IAsyncDisposable
{
    /// <summary>
    /// Gets the service provider for the pipeline.
    /// </summary>
    IServiceProvider Services { get; }

    /// <summary>
    /// Builds a dependency-ordered plan, including skip decisions and duration estimates, without executing modules.
    /// </summary>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <returns>The planned pipeline execution.</returns>
    Task<PipelinePlan> PlanAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Executes the pipeline and returns a summary of the results.
    /// </summary>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <returns>A summary of the pipeline execution results.</returns>
    Task<PipelineSummary> RunAsync(CancellationToken cancellationToken = default);
}
