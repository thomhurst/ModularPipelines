using ModularPipelines.Models;

namespace ModularPipelines.Helpers;

/// <summary>
/// Abstraction for printing pipeline execution results.
/// Separates results presentation from the progress tracking logic.
/// </summary>
internal interface IResultsPrinter
{
    /// <summary>
    /// Prints a summary of the pipeline execution results.
    /// </summary>
    /// <param name="pipelineSummary">The summary containing execution results.</param>
    void PrintResults(PipelineSummary pipelineSummary);
}
