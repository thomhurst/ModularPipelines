using System.Net;
using Amazon.S3;
using Amazon.S3.Model;
using ModularPipelines.Caching;
using ModularPipelines.Distributed.Artifacts.S3;
using ModularPipelines.Distributed.Artifacts.S3.Artifacts;

namespace ModularPipelines.Distributed.Artifacts.S3.Caching;

/// <summary>
/// Stores shareable module cache entries in S3 or an S3-compatible service.
/// Cache keys are independent of distributed pipeline run identifiers.
/// </summary>
public sealed class S3ModuleCache : IModuleCacheStore, IDisposable
{
    private readonly S3ArtifactOptions _options;
    private readonly long _maximumCacheEntryBytes;
    private readonly Lazy<IAmazonS3> _client;

    /// <summary>
    /// Initializes a new instance of the <see cref="S3ModuleCache"/> class.
    /// </summary>
    public S3ModuleCache(S3ArtifactOptions options)
        : this(options, new ModuleCacheOptions())
    {
    }

    internal S3ModuleCache(S3ArtifactOptions options, ModuleCacheOptions cacheOptions)
    {
        ValidateOptions(options);
        ValidateCacheOptions(cacheOptions);
        _options = options;
        _maximumCacheEntryBytes = cacheOptions.MaximumCacheEntryBytes;
        _client = new Lazy<IAmazonS3>(() => S3ClientFactory.Create(options));
    }

    internal S3ModuleCache(
        S3ArtifactOptions options,
        IAmazonS3 client,
        ModuleCacheOptions? cacheOptions = null)
    {
        ValidateOptions(options);
        ArgumentNullException.ThrowIfNull(client);
        cacheOptions ??= new ModuleCacheOptions();
        ValidateCacheOptions(cacheOptions);
        _options = options;
        _maximumCacheEntryBytes = cacheOptions.MaximumCacheEntryBytes;
        _client = new Lazy<IAmazonS3>(() => client);
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

        var request = new PutObjectRequest
        {
            BucketName = _options.BucketName,
            Key = BuildObjectKey(fingerprint),
            InputStream = content,
            ContentType = "application/zip",
            DisablePayloadSigning = true,
        };

        await _client.Value.PutObjectAsync(request, cancellationToken).ConfigureAwait(false);
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
        $"{_options.KeyPrefix.TrimEnd('/')}/module-cache/v1/{fingerprint.ToLowerInvariant()}.zip";

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

    private static void ValidateOptions(S3ArtifactOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        ArgumentException.ThrowIfNullOrWhiteSpace(options.BucketName);
        ArgumentException.ThrowIfNullOrWhiteSpace(options.KeyPrefix);
    }

    private static void ValidateCacheOptions(ModuleCacheOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        if (options.MaximumCacheEntryBytes <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(options),
                "ModuleCacheOptions.MaximumCacheEntryBytes must be positive.");
        }
    }
}
