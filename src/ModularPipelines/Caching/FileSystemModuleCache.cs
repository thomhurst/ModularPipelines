using Microsoft.Extensions.Options;

namespace ModularPipelines.Caching;

/// <summary>
/// Stores module cache entries atomically on the local filesystem.
/// </summary>
public sealed class FileSystemModuleCache : IModuleCacheStore
{
    private readonly string _cacheDirectory;

    /// <summary>
    /// Initialises a new instance of the <see cref="FileSystemModuleCache"/> class.
    /// </summary>
    public FileSystemModuleCache(IOptions<ModuleCacheOptions> options)
    {
        ArgumentNullException.ThrowIfNull(options);
        _cacheDirectory = Path.GetFullPath(options.Value.CacheDirectory);
    }

    /// <inheritdoc />
    public Task<Stream?> OpenReadAsync(string fingerprint, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var path = GetEntryPath(fingerprint);
        Stream? stream = File.Exists(path)
            ? new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, 64 * 1024, FileOptions.Asynchronous)
            : null;
        return Task.FromResult(stream);
    }

    /// <inheritdoc />
    public async Task WriteAsync(string fingerprint, Stream content, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(content);
        Directory.CreateDirectory(_cacheDirectory);

        var destination = GetEntryPath(fingerprint);
        var temporary = Path.Combine(_cacheDirectory, $".{fingerprint}.{Guid.NewGuid():N}.tmp");
        try
        {
            await using (var output = new FileStream(
                             temporary,
                             FileMode.CreateNew,
                             FileAccess.Write,
                             FileShare.None,
                             64 * 1024,
                             FileOptions.Asynchronous))
            {
                await content.CopyToAsync(output, cancellationToken).ConfigureAwait(false);
                await output.FlushAsync(cancellationToken).ConfigureAwait(false);
            }

            File.Move(temporary, destination, overwrite: true);
        }
        finally
        {
            File.Delete(temporary);
        }
    }

    private string GetEntryPath(string fingerprint)
    {
        ModuleCacheFingerprint.Validate(fingerprint);

        return Path.Combine(_cacheDirectory, $"{fingerprint.ToLowerInvariant()}.zip");
    }
}
