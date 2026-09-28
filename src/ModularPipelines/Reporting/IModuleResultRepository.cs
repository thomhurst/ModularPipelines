using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Reporting;

/// <summary>
/// Stores module results so that a later pipeline run can reuse them.
/// </summary>
/// <remarks>
/// <para>
/// Register an implementation with <c>AddResultsRepository&lt;TRepository&gt;()</c>. After a module
/// completes, the engine saves its result. When a module is skipped in a later run, the engine asks
/// the repository for a stored result and, when one is found, publishes it with the
/// <see cref="ModuleStatus.RestoredFromHistory"/> status so dependent modules can still read it.
/// </para>
/// <para>
/// This is the user-facing result history seam. Fingerprint-based incremental caching
/// (<c>AddModuleCache</c>) is a separate feature with its own storage, and module durations used for
/// progress estimates are provided by <see cref="IModuleEstimatedTimeProvider"/>.
/// </para>
/// <para>
/// Repository failures are best-effort: the engine logs exceptions from these members and continues
/// as if no result was stored.
/// </para>
/// </remarks>
public interface IModuleResultRepository
{
    /// <summary>
    /// Gets a value indicating whether this repository is enabled and should be used for storing/retrieving results.
    /// </summary>
    /// <remarks>
    /// When <c>false</c>, the pipeline skips history-related operations. The default is <see langword="true"/>.
    /// </remarks>
    bool IsEnabled => true;

    /// <summary>
    /// Saves the final result of a module.
    /// </summary>
    /// <typeparam name="T">The module's result value type.</typeparam>
    /// <param name="module">The module that produced the result.</param>
    /// <param name="moduleResult">The result to store.</param>
    /// <param name="pipelineContext">The pipeline context.</param>
    /// <param name="cancellationToken">A token that cancels the operation.</param>
    /// <returns>A task representing the save operation.</returns>
    Task SaveResultAsync<T>(
        Module<T> module,
        ModuleResult<T> moduleResult,
        IPipelineContext pipelineContext,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a previously stored result for a module.
    /// </summary>
    /// <typeparam name="T">The module's result value type.</typeparam>
    /// <param name="module">The module whose result is requested.</param>
    /// <param name="pipelineContext">The pipeline context.</param>
    /// <param name="cancellationToken">A token that cancels the operation.</param>
    /// <returns>The stored result, or <see langword="null"/> when none is available.</returns>
    Task<ModuleResult<T>?> GetResultAsync<T>(
        Module<T> module,
        IPipelineContext pipelineContext,
        CancellationToken cancellationToken = default);
}
