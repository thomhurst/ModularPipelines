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
    /// <param name="fingerprint">The entry fingerprint.</param>
    /// <param name="cancellationToken">A token used to cancel the read.</param>
    /// <returns>A readable stream containing the cache entry, or <see langword="null"/> when it does not exist.</returns>
    Task<Stream?> OpenReadAsync(string fingerprint, CancellationToken cancellationToken);

    /// <summary>
    /// Writes or replaces a cached entry.
    /// </summary>
    /// <param name="fingerprint">The entry fingerprint.</param>
    /// <param name="content">The entry contents.</param>
    /// <param name="cancellationToken">A token used to cancel the write.</param>
    /// <returns>A task representing the asynchronous write operation.</returns>
    Task WriteAsync(string fingerprint, Stream content, CancellationToken cancellationToken);

    /// <summary>
    /// Returns whether a cached entry exists.
    /// </summary>
    /// <param name="fingerprint">The entry fingerprint.</param>
    /// <param name="cancellationToken">A token used to cancel the check.</param>
    /// <returns><see langword="true"/> when the entry exists.</returns>
    /// <remarks>
    /// The default implementation opens the entry with <see cref="OpenReadAsync"/> and disposes it.
    /// Override it when the store can check existence without reading the entry.
    /// </remarks>
    async Task<bool> ExistsAsync(string fingerprint, CancellationToken cancellationToken)
    {
        var stream = await OpenReadAsync(fingerprint, cancellationToken).ConfigureAwait(false);
        if (stream is null)
        {
            return false;
        }

        await stream.DisposeAsync().ConfigureAwait(false);
        return true;
    }

    /// <summary>
    /// Deletes a cached entry. Deleting an entry that does not exist does nothing.
    /// </summary>
    /// <param name="fingerprint">The entry fingerprint.</param>
    /// <param name="cancellationToken">A token used to cancel the deletion.</param>
    /// <returns>A task representing the asynchronous delete operation.</returns>
    Task DeleteAsync(string fingerprint, CancellationToken cancellationToken);
}
