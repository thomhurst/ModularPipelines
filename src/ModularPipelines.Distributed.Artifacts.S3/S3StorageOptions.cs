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
    public string? AccessKey { get; init; }

    /// <summary>
    /// Gets the secret key. Required for non-AWS providers or explicit auth.
    /// Set together with <see cref="AccessKey"/>.
    /// </summary>
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
    /// Gets whether to add or update a bucket lifecycle rule that expires artifacts under
    /// <c>{KeyPrefix}/artifacts/</c> after <see cref="TimeToLive"/>. Existing rules are preserved.
    /// Requires the <c>s3:GetLifecycleConfiguration</c> and <c>s3:PutLifecycleConfiguration</c>
    /// permissions; failures are logged as warnings. Default: false.
    /// </summary>
    public bool SetLifecycleRule { get; init; }

    /// <summary>
    /// Gets how long artifacts are kept when <see cref="SetLifecycleRule"/> is enabled.
    /// S3 lifecycle expiration is day-granular, so this rounds up to whole days. Default: 1 day.
    /// </summary>
    public TimeSpan TimeToLive { get; init; } = TimeSpan.FromDays(1);
}
