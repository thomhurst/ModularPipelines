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
    Task<Stream?> OpenReadAsync(string fingerprint, CancellationToken cancellationToken);

    /// <summary>
    /// Writes or replaces a cached entry.
    /// </summary>
    Task WriteAsync(string fingerprint, Stream content, CancellationToken cancellationToken);
}
