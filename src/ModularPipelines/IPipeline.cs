using ModularPipelines.Enums;
using ModularPipelines.Models;
using ModularPipelines.Reporting;

namespace ModularPipelines;

/// <summary>
/// Represents a built pipeline ready for execution.
/// </summary>
/// <remarks>
/// Modules registered through the pipeline builder remain alive until the pipeline is disposed,
/// including after <see cref="RunAsync"/> completes. Dispose the pipeline with <c>await using</c>
/// to release module resources, including resources held by modules that did not execute.
/// </remarks>
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
    /// <remarks>
    /// Exceptions containing registered secrets are replaced with sanitized snapshots, including
    /// their inner exceptions. Concrete exception types can change; aggregate exceptions and
    /// cancellation semantics are preserved. Exceptions that need no masking are rethrown unchanged.
    /// </remarks>
    Task<PipelineSummary> RunAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Exports the resolved module dependency graph without executing modules.
    /// </summary>
    /// <param name="format">The output format.</param>
    /// <param name="path">The destination file.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <exception cref="Exceptions.PipelineException">
    /// Thrown when pipeline execution has already started.
    /// </exception>
    Task ExportDependencyGraphAsync(
        DependencyGraphFormat format,
        string path,
        CancellationToken cancellationToken = default);
}
