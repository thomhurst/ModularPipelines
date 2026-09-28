namespace ModularPipelines.Distributed.Artifacts.S3;

/// <summary>
/// Configuration shared by every S3-backed feature in this package: the distributed artifact
/// store and the S3 module cache. Works with AWS S3, Cloudflare R2, Backblaze B2, and MinIO.
/// </summary>
public class S3StorageOptions
{
    /// <summary>
    /// Gets or sets the S3 bucket name. Required.
    /// </summary>
    public string BucketName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the S3-compatible service URL.
    /// Omit for AWS S3 (uses default endpoints).
    /// Set for Cloudflare R2: "https://{account_id}.r2.cloudflarestorage.com"
    /// Set for Backblaze B2: "https://s3.{region}.backblazeb2.com"
    /// Set for MinIO: "http://localhost:9000"
    /// </summary>
    public string? ServiceUrl { get; set; }

    /// <summary>
    /// Gets or sets the access key. Required for non-AWS providers or explicit auth.
    /// Set together with <see cref="SecretKey"/>.
    /// </summary>
    public string? AccessKey { get; set; }

    /// <summary>
    /// Gets or sets the secret key. Required for non-AWS providers or explicit auth.
    /// Set together with <see cref="AccessKey"/>.
    /// </summary>
    public string? SecretKey { get; set; }

    /// <summary>
    /// Gets or sets the AWS region. Default: us-east-1.
    /// </summary>
    public string Region { get; set; } = "us-east-1";

    /// <summary>
    /// Gets or sets whether to use path-style addressing.
    /// Required for MinIO and some S3-compatible providers. Default: false.
    /// </summary>
    public bool ForcePathStyle { get; set; }

    /// <summary>
    /// Gets or sets the prefix for every object key. Artifacts are stored under
    /// <c>{KeyPrefix}/artifacts/</c> and module cache entries under <c>{KeyPrefix}/module-cache/</c>.
    /// Default: <c>modpipe</c>.
    /// </summary>
    public string KeyPrefix { get; set; } = "modpipe";

    /// <summary>
    /// Gets or sets the size of each part when uploading large objects. Content no larger than one
    /// part is uploaded with a single request; larger content uses a multipart upload, which S3
    /// limits to 10,000 parts. Must be between 5 MB and 1 GB. Default: 16 MB.
    /// </summary>
    public int MultipartPartSizeBytes { get; set; } = 16 * 1024 * 1024;

    /// <summary>
    /// Gets or sets whether to add or update a bucket lifecycle rule that expires artifacts under
    /// <c>{KeyPrefix}/artifacts/</c> after <see cref="TimeToLive"/>. Existing rules are preserved.
    /// Requires the <c>s3:GetLifecycleConfiguration</c> and <c>s3:PutLifecycleConfiguration</c>
    /// permissions; failures are logged as warnings. Default: false.
    /// </summary>
    public bool SetLifecycleRule { get; set; }

    /// <summary>
    /// Gets or sets how long artifacts are kept when <see cref="SetLifecycleRule"/> is enabled.
    /// S3 lifecycle expiration is day-granular, so this rounds up to whole days. Default: 1 day.
    /// </summary>
    public TimeSpan TimeToLive { get; set; } = TimeSpan.FromDays(1);
}
