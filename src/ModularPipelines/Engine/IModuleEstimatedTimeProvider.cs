using ModularPipelines.Models;

namespace ModularPipelines.Engine;

/// <summary>
/// Provides functionality to track and estimate module execution times.
/// </summary>
public interface IModuleEstimatedTimeProvider
{
    /// <summary>
    /// Gets the estimated execution time for a module type.
    /// </summary>
    /// <param name="moduleType">The type of module to get estimated time for.</param>
    /// <returns>A task that represents the asynchronous operation. The value contains the estimated execution time.</returns>
    Task<TimeSpan> GetModuleEstimatedTimeAsync(Type moduleType);

    /// <summary>
    /// Saves the actual execution time for a module type to improve future estimates.
    /// </summary>
    /// <param name="moduleType">The type of module that was executed.</param>
    /// <param name="duration">The actual duration of the module execution.</param>
    /// <returns>A task that represents the asynchronous save operation.</returns>
    Task SaveModuleTimeAsync(Type moduleType, TimeSpan duration);

    /// <summary>
    /// Gets the estimated execution times for sub-operations of a module type.
    /// </summary>
    /// <param name="moduleType">The type of module to get sub-operation estimates for.</param>
    /// <returns>A task that represents the asynchronous operation. The value contains the sub-operation estimations.</returns>
    Task<IEnumerable<SubModuleEstimation>> GetSubModuleEstimatedTimesAsync(Type moduleType);

    /// <summary>
    /// Saves the actual execution time for a sub-operation to improve future estimates.
    /// </summary>
    /// <param name="moduleType">The type of module containing the sub-operation.</param>
    /// <param name="subModuleEstimation">The sub-operation estimation data.</param>
    /// <returns>A task that represents the asynchronous save operation.</returns>
    Task SaveSubModuleTimeAsync(Type moduleType, SubModuleEstimation subModuleEstimation);
}
