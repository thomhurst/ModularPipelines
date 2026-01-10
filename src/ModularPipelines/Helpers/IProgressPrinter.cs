using ModularPipelines.Models;

namespace ModularPipelines.Helpers;

/// <summary>
/// Coordinates progress display and results printing for pipeline execution.
/// </summary>
internal interface IProgressPrinter
{
    /// <summary>
    /// Displays real-time progress of module execution.
    /// </summary>
    /// <param name="organizedModules">The organized modules to track.</param>
    /// <param name="cancellationToken">Token to signal cancellation.</param>
    Task PrintProgress(OrganizedModules organizedModules, CancellationToken cancellationToken);

    /// <summary>
    /// Prints the final results summary after pipeline completion.
    /// </summary>
    /// <param name="pipelineSummary">The pipeline execution summary.</param>
    void PrintResults(PipelineSummary pipelineSummary);
}
