using ModularPipelines.Models;

namespace ModularPipelines.Distributed.Abstractions;

/// <summary>
/// Interface for caching module results in a distributed system.
/// </summary>
public interface IResultCache
{
    /// <summary>
    /// Stores a module result in the cache.
    /// </summary>
    /// <param name="moduleType">The type of the module.</param>
    /// <param name="result">The module result to cache.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task SetResultAsync(
        Type moduleType,
        IModuleResult result,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a module result from the cache.
    /// </summary>
    /// <param name="moduleType">The type of the module.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The cached result, or null if not found.</returns>
    Task<IModuleResult?> GetResultAsync(
        Type moduleType,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if a result exists in the cache for the specified module type.
    /// </summary>
    /// <param name="moduleType">The type of the module.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>True if the result exists; otherwise, false.</returns>
    Task<bool> ContainsResultAsync(
        Type moduleType,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Clears all cached results.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task ClearAsync(CancellationToken cancellationToken = default);
}
