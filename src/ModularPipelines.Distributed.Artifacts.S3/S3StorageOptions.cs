using System.Text;
using ModularPipelines.Secrets;

namespace ModularPipelines.Distributed.Artifacts.S3;

/// <summary>
/// Configuration shared by every S3-backed feature in this package: the distributed artifact
/// store and the S3 module cache. Works with AWS S3, Cloudflare R2, Backblaze B2, and MinIO.
/// </summary>
public sealed record S3StorageOptions
{
    /// <summary>
    /// Gets the S3 bucket name. Required.
    /// </summary>
    public string BucketName { get; init; } = string.Empty;

    /// <summary>
    /// Gets the absolute HTTP or HTTPS S3-compatible service URL.
    /// Omit for AWS S3 (uses default endpoints). Blank values are treated as omitted.
    /// Set for Cloudflare R2: "https://{account_id}.r2.cloudflarestorage.com"
    /// Set for Backblaze B2: "https://s3.{region}.backblazeb2.com"
    /// Set for MinIO: "http://localhost:9000"
    /// </summary>
    public Uri? ServiceUrl
    {
        get;
        init => field = string.IsNullOrWhiteSpace(value?.OriginalString) ? null : value;
    }

    /// <summary>
    /// Gets the access key. Required for non-AWS providers or explicit auth.
    /// Set together with <see cref="SecretKey"/>.
    /// </summary>
    [SecretValue]
    public string? AccessKey { get; init; }

    /// <summary>
    /// Gets the secret key. Required for non-AWS providers or explicit auth.
    /// Set together with <see cref="AccessKey"/>.
    /// </summary>
    [SecretValue]
    public string? SecretKey { get; init; }

    /// <summary>
    /// Gets the AWS region. Default: us-east-1.
    /// </summary>
    public string Region { get; init; } = "us-east-1";

    /// <summary>
    /// Gets whether to use path-style addressing.
    /// Required for MinIO and some S3-compatible providers. Default: false.
    /// </summary>
    public bool ForcePathStyle { get; init; }

    /// <summary>
    /// Gets the prefix for every object key. Artifacts are stored under
    /// <c>{KeyPrefix}/artifacts/</c> and module cache entries under <c>{KeyPrefix}/module-cache/</c>.
    /// Default: <c>modpipe</c>.
    /// </summary>
    public string KeyPrefix { get; init; } = "modpipe";

    /// <summary>
    /// Gets the size of each part when uploading large objects. Content no larger than one
    /// part is uploaded with a single request; larger content uses a multipart upload, which S3
    /// limits to 10,000 parts. Must be between 5 MB and 1 GB. Default: 16 MB.
    /// </summary>
    public int MultipartPartSizeBytes { get; init; } = 16 * 1024 * 1024;

    /// <summary>
    /// Gets whether to check an externally provisioned artifact lifecycle rule at startup.
    /// Despite its historical name, this option never writes bucket lifecycle configuration.
    /// The expected rule ID is <c>modpipe-artifact-expiration:{KeyPrefix}/artifacts/</c>, with
    /// leading and trailing slashes trimmed from <see cref="KeyPrefix"/>. It must be enabled,
    /// filter that artifact prefix, expire objects after <see cref="TimeToLive"/> rounded up to
    /// whole days (minimum one day), and abort incomplete multipart uploads after one day.
    /// Requires <c>s3:GetLifecycleConfiguration</c>; a missing or different rule or a read failure
    /// logs a warning and allows startup to continue. Provision rules through your infrastructure
    /// owner before startup. Default: false (no lifecycle API requests).
    /// </summary>
    public bool SetLifecycleRule { get; init; }

    /// <summary>
    /// Gets the expected artifact lifetime checked when <see cref="SetLifecycleRule"/> is enabled.
    /// Rounds up to whole days, with a minimum of one day. Does not change the externally
    /// provisioned lifecycle rule or expire objects itself. Default: 1 day.
    /// </summary>
    public TimeSpan TimeToLive { get; init; } = TimeSpan.FromDays(1);

    private bool PrintMembers(StringBuilder builder)
    {
        builder.Append("BucketName = ").Append(BucketName);
        builder.Append(", ServiceUrl = ").Append(ServiceUrl is null ? "<none>" : "<redacted>");
        builder.Append(", AccessKey = ").Append(AccessKey is null ? "<none>" : "<redacted>");
        builder.Append(", SecretKey = ").Append(SecretKey is null ? "<none>" : "<redacted>");
        builder.Append(", Region = ").Append(Region);
        builder.Append(", ForcePathStyle = ").Append(ForcePathStyle);
        builder.Append(", KeyPrefix = ").Append(KeyPrefix);
        builder.Append(", MultipartPartSizeBytes = ").Append(MultipartPartSizeBytes);
        builder.Append(", SetLifecycleRule = ").Append(SetLifecycleRule);
        builder.Append(", TimeToLive = ").Append(TimeToLive);
        return true;
    }
}
