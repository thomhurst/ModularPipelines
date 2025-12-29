using ModularPipelines.Models;

namespace ModularPipelines.Engine.Executors;

/// <summary>
/// Coordinates all pipeline output operations including progress printing,
/// module output, results display, and exception flushing.
/// </summary>
internal interface IPipelineOutputCoordinator
{
    /// <summary>
    /// Initializes the output coordinator for a new pipeline execution.
    /// Returns a scope that manages the lifecycle of output resources.
    /// </summary>
    Task<IPipelineOutputScope> InitializeAsync();

    /// <summary>
    /// Prints the final pipeline results.
    /// </summary>
    void PrintResults(PipelineSummary pipelineSummary);

    /// <summary>
    /// Flushes any buffered exceptions to the console.
    /// </summary>
    void FlushExceptions();

    /// <summary>
    /// Writes accumulated logs after pipeline completion.
    /// </summary>
    void WriteLogs();
}
