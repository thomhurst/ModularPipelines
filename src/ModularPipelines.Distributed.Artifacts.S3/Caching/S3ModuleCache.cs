using System.Net;
using Amazon.S3;
using Amazon.S3.Model;
using ModularPipelines.Caching;
using ModularPipelines.Distributed.Artifacts.S3.Artifacts;

namespace ModularPipelines.Distributed.Artifacts.S3.Caching;

/// <summary>
/// Stores shareable module cache entries in S3 or an S3-compatible service.
/// Cache keys are independent of distributed pipeline run identifiers.
/// </summary>
internal sealed class S3ModuleCache : IModuleCacheStore, IDisposable
{
    private readonly S3StorageOptions _options;
    private readonly long _maximumCacheEntryBytes;
    private readonly Lazy<IAmazonS3> _client;

    public S3ModuleCache(S3StorageOptions options, ModuleCacheOptions cacheOptions)
        : this(options, cacheOptions, () => S3ClientFactory.Create(options))
    {
    }

    internal S3ModuleCache(
        S3StorageOptions options,
        IAmazonS3 client,
        ModuleCacheOptions? cacheOptions = null)
        : this(options, cacheOptions ?? new ModuleCacheOptions(), () => client)
    {
        ArgumentNullException.ThrowIfNull(client);
    }

    private S3ModuleCache(S3StorageOptions options, ModuleCacheOptions cacheOptions, Func<IAmazonS3> createClient)
    {
        ValidateOptions(options);
        ValidateCacheOptions(cacheOptions);
        _options = options;
        _maximumCacheEntryBytes = cacheOptions.MaxCacheEntryBytes;
        _client = new Lazy<IAmazonS3>(createClient);
    }

    /// <inheritdoc />
    public async Task<Stream?> OpenReadAsync(string fingerprint, CancellationToken cancellationToken)
    {
        ModuleCacheFingerprint.Validate(fingerprint);

        GetObjectResponse response;
        try
        {
            response = await _client.Value
                .GetObjectAsync(_options.BucketName, BuildObjectKey(fingerprint), cancellationToken)
                .ConfigureAwait(false);
        }
        catch (AmazonS3Exception exception) when (exception.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        using (response)
        {
            if (response.ContentLength > _maximumCacheEntryBytes)
            {
                throw CreateEntryLimitException();
            }

            var temporary = Path.GetTempFileName();
            var stream = new FileStream(
                temporary,
                FileMode.Create,
                FileAccess.ReadWrite,
                FileShare.None,
                64 * 1024,
                FileOptions.Asynchronous | FileOptions.DeleteOnClose);
            try
            {
                await CopyResponseToAsync(response.ResponseStream, stream, cancellationToken)
                    .ConfigureAwait(false);
                stream.Position = 0;
                return stream;
            }
            catch
            {
                await stream.DisposeAsync().ConfigureAwait(false);
                throw;
            }
        }
    }

    /// <inheritdoc />
    public async Task WriteAsync(string fingerprint, Stream content, CancellationToken cancellationToken)
    {
        ModuleCacheFingerprint.Validate(fingerprint);
        ArgumentNullException.ThrowIfNull(content);

        await S3ObjectUploader.UploadAsync(
                _client.Value,
                _options.BucketName,
                BuildObjectKey(fingerprint),
                content,
                "application/zip",
                metadata: null,
                _options.MultipartPartSizeBytes,
                cancellationToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<bool> ExistsAsync(string fingerprint, CancellationToken cancellationToken)
    {
        ModuleCacheFingerprint.Validate(fingerprint);
        try
        {
            await _client.Value
                .GetObjectMetadataAsync(_options.BucketName, BuildObjectKey(fingerprint), cancellationToken)
                .ConfigureAwait(false);
            return true;
        }
        catch (AmazonS3Exception exception) when (exception.StatusCode == HttpStatusCode.NotFound)
        {
            return false;
        }
    }

    /// <inheritdoc />
    public async Task DeleteAsync(string fingerprint, CancellationToken cancellationToken)
    {
        ModuleCacheFingerprint.Validate(fingerprint);
        await _client.Value
            .DeleteObjectAsync(_options.BucketName, BuildObjectKey(fingerprint), cancellationToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc />
    public void Dispose()
    {
        if (_client.IsValueCreated)
        {
            _client.Value.Dispose();
        }
    }

    private string BuildObjectKey(string fingerprint) =>
        $"{_options.KeyPrefix.Trim('/')}/module-cache/v1/{fingerprint.ToLowerInvariant()}.zip";

    private async Task CopyResponseToAsync(
        Stream input,
        Stream output,
        CancellationToken cancellationToken)
    {
        const int bufferSize = 64 * 1024;
        var buffer = new byte[bufferSize];
        var totalBytes = 0L;
        while (true)
        {
            var bytesRead = await input.ReadAsync(buffer, cancellationToken).ConfigureAwait(false);
            if (bytesRead == 0)
            {
                return;
            }

            if (totalBytes > _maximumCacheEntryBytes - bytesRead)
            {
                throw CreateEntryLimitException();
            }

            totalBytes += bytesRead;
            await output.WriteAsync(buffer.AsMemory(0, bytesRead), cancellationToken)
                .ConfigureAwait(false);
        }
    }

    private InvalidDataException CreateEntryLimitException() =>
        new($"S3 module cache entry exceeded the configured limit of {_maximumCacheEntryBytes:N0} bytes.");

    private static void ValidateOptions(S3StorageOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        ArgumentException.ThrowIfNullOrWhiteSpace(options.BucketName);
        ArgumentException.ThrowIfNullOrWhiteSpace(options.KeyPrefix);
    }

    private static void ValidateCacheOptions(ModuleCacheOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        if (options.MaxCacheEntryBytes <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(options),
                "ModuleCacheOptions.MaxCacheEntryBytes must be positive.");
        }
    }
}
