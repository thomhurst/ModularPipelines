using ModularPipelines.Models;

namespace ModularPipelines.Extensions;

/// <summary>
/// Extension methods for IPipeline for backward compatibility.
/// </summary>
public static class PipelineExtensions
{
    /// <summary>
    /// Executes the pipeline and returns a summary.
    /// </summary>
    /// <param name="pipeline">The pipeline to execute.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <returns>A summary of the pipeline execution results.</returns>
    /// <remarks>This is an alias for <see cref="IPipeline.RunAsync"/> for backward compatibility.</remarks>
    public static Task<PipelineSummary> ExecutePipelineAsync(this IPipeline pipeline, CancellationToken cancellationToken = default)
    {
        return pipeline.RunAsync(cancellationToken);
    }
}
