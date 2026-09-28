using ModularPipelines.Models;

namespace ModularPipelines.Reporting;

/// <summary>
/// Stores and estimates module and sub-module execution times for progress display and scheduling.
/// </summary>
/// <remarks>
/// <para>
/// ModularPipelines registers a default file-system provider that keeps the last measured duration of
/// each module under the user's application-data folder. Register a replacement with
/// <c>AddModuleEstimatedTimeProvider&lt;TProvider&gt;()</c>, for example to derive estimates from
/// <see cref="IRunHistoryReader.GetModuleDurationTrendAsync"/> or a shared store.
/// </para>
/// <para>
/// Estimates are best-effort. The engine logs exceptions thrown by a provider and falls back to a
/// default estimate or skips the save, so provider failures never change a module's outcome.
/// Members with a sensible default are implemented by the interface; override them when the
/// provider stores sub-module timings.
/// </para>
/// </remarks>
public interface IModuleEstimatedTimeProvider
{
    /// <summary>
    /// Gets the estimated execution time for a module type.
    /// </summary>
    /// <param name="moduleType">The type of module to get estimated time for.</param>
    /// <param name="cancellationToken">A token that cancels the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The value contains the estimated execution time.</returns>
    Task<TimeSpan> GetModuleEstimatedTimeAsync(Type moduleType, CancellationToken cancellationToken = default);

    /// <summary>
    /// Saves the actual execution time for a module type to improve future estimates.
    /// </summary>
    /// <param name="moduleType">The type of module that was executed.</param>
    /// <param name="duration">The actual duration of the module execution.</param>
    /// <param name="cancellationToken">A token that cancels the operation.</param>
    /// <returns>A task that represents the asynchronous save operation.</returns>
    Task SaveModuleTimeAsync(Type moduleType, TimeSpan duration, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the estimated execution times for sub-operations of a module type.
    /// </summary>
    /// <param name="moduleType">The type of module to get sub-operation estimates for.</param>
    /// <param name="cancellationToken">A token that cancels the operation.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The value contains the sub-operation estimations.
    /// The default implementation returns no estimations.
    /// </returns>
    Task<IEnumerable<SubModuleEstimation>> GetSubModuleEstimatedTimesAsync(
        Type moduleType,
        CancellationToken cancellationToken = default) =>
        Task.FromResult<IEnumerable<SubModuleEstimation>>([]);

    /// <summary>
    /// Saves the actual execution time for a sub-operation to improve future estimates.
    /// </summary>
    /// <param name="moduleType">The type of module containing the sub-operation.</param>
    /// <param name="subModuleEstimation">The sub-operation estimation data.</param>
    /// <param name="cancellationToken">A token that cancels the operation.</param>
    /// <returns>A task that represents the asynchronous save operation. The default implementation does nothing.</returns>
    Task SaveSubModuleTimeAsync(
        Type moduleType,
        SubModuleEstimation subModuleEstimation,
        CancellationToken cancellationToken = default) =>
        Task.CompletedTask;
}
