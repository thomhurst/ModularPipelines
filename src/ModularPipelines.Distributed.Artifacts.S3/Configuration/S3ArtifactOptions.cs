namespace ModularPipelines.Distributed.Artifacts.S3.Configuration;

/// <summary>
/// Configuration options for the S3-compatible distributed artifact store.
/// Works with AWS S3, Cloudflare R2, Backblaze B2, and MinIO.
/// </summary>
public class S3ArtifactOptions
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
    /// </summary>
    public string? AccessKey { get; set; }

    /// <summary>
    /// Gets or sets the secret key. Required for non-AWS providers or explicit auth.
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
    /// Gets or sets the key prefix for artifact objects. Default: "modpipe-artifacts".
    /// </summary>
    public string KeyPrefix { get; set; } = "modpipe-artifacts";

    /// <summary>
    /// Gets or sets the run identifier for key isolation.
    /// If not set, auto-detected from CI environment variables or git SHA.
    /// </summary>
    public string? RunIdentifier { get; set; }

    /// <summary>
    /// Gets or sets whether to auto-configure a lifecycle rule on the bucket for artifact expiration.
    /// Default: true.
    /// </summary>
    public bool SetLifecycleRule { get; set; } = true;
}
