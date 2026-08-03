using ModularPipelines.Enums;

namespace ModularPipelines.Engine;

/// <summary>
/// Renders and exports the pipeline's resolved module dependency graph.
/// </summary>
public interface IDependencyGraphExporter
{
    /// <summary>
    /// Renders the dependency graph in the requested format.
    /// </summary>
    Task<string> RenderAsync(
        DependencyGraphFormat format,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Writes the dependency graph to a file.
    /// </summary>
    Task ExportAsync(
        DependencyGraphFormat format,
        string path,
        CancellationToken cancellationToken = default);
}
