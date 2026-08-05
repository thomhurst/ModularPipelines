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
        try
        {
            Stream stream = new FileStream(
                path,
                FileMode.Open,
                FileAccess.Read,
                FileShare.Read | FileShare.Delete,
                64 * 1024,
                FileOptions.Asynchronous);
            return Task.FromResult<Stream?>(stream);
        }
        catch (FileNotFoundException)
        {
            return Task.FromResult<Stream?>(null);
        }
        catch (DirectoryNotFoundException)
        {
            return Task.FromResult<Stream?>(null);
        }
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

            Publish(temporary, destination);
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

    private static void Publish(string temporary, string destination)
    {
        if (!OperatingSystem.IsWindows())
        {
            File.Move(temporary, destination, overwrite: true);
            return;
        }

        var backup = $"{temporary}.bak";
        try
        {
            try
            {
                File.Replace(temporary, destination, backup, ignoreMetadataErrors: true);
            }
            catch (FileNotFoundException)
            {
                File.Move(temporary, destination, overwrite: true);
            }
        }
        finally
        {
            File.Delete(backup);
        }
    }
}
