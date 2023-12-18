using ModularPipelines.Models;

namespace ModularPipelines.Engine.Executors;

/// <summary>
/// Orchestrates executing the pipeline, with logging and exception handling.
/// </summary>
internal interface IExecutionOrchestrator
{
    /// <summary>
    /// Executes the pipeline.
    /// </summary>
    /// <param name="cancellationToken">An optional cancellation token.</param>
    /// <returns>A summary of the pipeline.</returns>
    Task<PipelineSummary> ExecuteAsync(CancellationToken cancellationToken = default);
}