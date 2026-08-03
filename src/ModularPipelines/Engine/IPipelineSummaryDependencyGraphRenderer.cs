using ModularPipelines.Enums;
using ModularPipelines.Models;

namespace ModularPipelines.Engine;

/// <summary>
/// Renders a dependency graph from completed pipeline results.
/// </summary>
public interface IPipelineSummaryDependencyGraphRenderer
{
    /// <summary>
    /// Renders the dependency graph using completed results for status annotations.
    /// </summary>
    Task<string> RenderAsync(
        DependencyGraphFormat format,
        PipelineSummary pipelineSummary,
        CancellationToken cancellationToken = default);
}
