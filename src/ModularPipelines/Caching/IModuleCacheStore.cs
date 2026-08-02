namespace ModularPipelines.Caching;

/// <summary>
/// Stores opaque module cache entries by their SHA-256 fingerprint.
/// </summary>
public interface IModuleCacheStore
{
    /// <summary>
    /// Opens a cached entry for reading, or returns <see langword="null"/> when it does not exist.
    /// The caller owns the returned stream.
    /// </summary>
    /// <returns>A readable stream containing the cache entry, or <see langword="null"/> when it does not exist.</returns>
    Task<Stream?> OpenReadAsync(string fingerprint, CancellationToken cancellationToken);

    /// <summary>
    /// Writes or replaces a cached entry.
    /// </summary>
    /// <returns>A task representing the asynchronous write operation.</returns>
    Task WriteAsync(string fingerprint, Stream content, CancellationToken cancellationToken);
}
