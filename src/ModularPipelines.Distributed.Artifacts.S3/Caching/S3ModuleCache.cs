using System.Net;
using Amazon.S3;
using Amazon.S3.Model;
using ModularPipelines.Caching;
using ModularPipelines.Distributed.Artifacts.S3.Artifacts;
using ModularPipelines.Distributed.Artifacts.S3.Configuration;

namespace ModularPipelines.Distributed.Artifacts.S3.Caching;

/// <summary>
/// Stores shareable module cache entries in S3 or an S3-compatible service.
/// Cache keys are independent of distributed pipeline run identifiers.
/// </summary>
public sealed class S3ModuleCache : IModuleCacheStore, IDisposable
{
    private readonly S3ArtifactOptions _options;
    private readonly Lazy<IAmazonS3> _client;

    /// <summary>
    /// Initialises a new instance of the <see cref="S3ModuleCache"/> class.
    /// Initializes a new instance of the <see cref="S3ModuleCache"/> class.
    /// </summary>
    public S3ModuleCache(S3ArtifactOptions options)
    {
        ValidateOptions(options);
        _options = options;
        _client = new Lazy<IAmazonS3>(() => S3ClientFactory.Create(options));
    }

    internal S3ModuleCache(S3ArtifactOptions options, IAmazonS3 client)
    {
        ValidateOptions(options);
        ArgumentNullException.ThrowIfNull(client);
        _options = options;
        _client = new Lazy<IAmazonS3>(() => client);
    }

    /// <inheritdoc />
    public async Task<Stream?> OpenReadAsync(string fingerprint, CancellationToken cancellationToken)
    {
        ValidateFingerprint(fingerprint);

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
                await response.ResponseStream.CopyToAsync(stream, cancellationToken).ConfigureAwait(false);
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
        ValidateFingerprint(fingerprint);
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

    private static void ValidateFingerprint(string fingerprint)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(fingerprint);
        if (fingerprint.Length != 64 || fingerprint.Any(character => !Uri.IsHexDigit(character)))
        {
            throw new ArgumentException("A module cache fingerprint must be a 64-character SHA-256 value.", nameof(fingerprint));
        }
    }

    private static void ValidateOptions(S3ArtifactOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        ArgumentException.ThrowIfNullOrWhiteSpace(options.BucketName);
        ArgumentException.ThrowIfNullOrWhiteSpace(options.KeyPrefix);
    }
}
