---
title: Distributed S3 Artifacts Package
---

# Distributed S3 Artifacts Package

`ModularPipelines.Distributed.Artifacts.S3` stores distributed pipeline artifacts in AWS S3 or an S3-compatible service such as Cloudflare R2, Backblaze B2, or MinIO.

## Installation

```shell
dotnet add package ModularPipelines.Distributed.Artifacts.S3
```

## Configuration

Register the artifact store after enabling distributed mode:

```csharp
using ModularPipelines.Distributed;
using ModularPipelines.Distributed.Artifacts.S3;

var builder = Pipeline.CreateBuilder(args);

builder.AddDistributedMode(options => options with { TotalInstances = 2 });
builder.AddS3DistributedArtifactStore(options => options with
{
    BucketName = "pipeline-artifacts",
    Region = "eu-west-2",
});
```

Modules publish and download artifacts through the context property. Cancellation
tokens are optional, and the typed download overload identifies the producer without
a string module name:

```csharp
await context.Artifacts.PublishFileAsync("package", packagePath);
await context.Artifacts.DownloadAsync<BuildModule>(
    "package",
    Path.Combine(context.Environment.WorkingDirectory.Path, "package.zip"));
```

### Typed file and directory paths

All artifact stores support `FilePath` and `FolderPath` overloads. Use paths from
`context.Files` to retain the pipeline working directory and configured filesystem:

```csharp
var package = context.Files.GetFile("output/package.zip");
await context.Artifacts.PublishFileAsync("package", package, cancellationToken);

var downloaded = await context.Artifacts.DownloadAsync<BuildModule>(
    "package", context.Files.GetFile("downloads/package.zip"), cancellationToken);

var output = context.Files.GetFolder("output");
await context.Artifacts.PublishDirectoryAsync("output", output, cancellationToken);
var extracted = await context.Artifacts.DownloadAsync<BuildModule>(
    "output", context.Files.GetFolder("downloads/output"), cancellationToken);
```

Downloads return the supplied `FilePath` or `FolderPath`, retaining its provider.
Both generic producer overloads and overloads accepting a `ModuleId` support typed
paths. A file artifact requires a `FilePath` destination; a published directory
requires a `FolderPath` destination. A mismatch throws before writing the download.
A ZIP file published with `PublishFileAsync` is still a file artifact.

Typed operations use the path's already-resolved absolute location and its own
filesystem provider, including custom providers. Constructing `new FilePath(...)`
or `new FolderPath(...)` directly uses the process working directory and system
filesystem. Existing string overloads keep their system-filesystem behavior.
Custom providers declare their relative-path separator through
`IFileSystemProvider.DirectorySeparatorChar`, which defaults to the host separator.
A provider using backslash separators on Unix must override it. Artifact ZIPs normalize
that separator before extraction containment checks; slash-based Unix providers keep
literal backslashes in filenames. Typed paths still require host-compatible absolute roots.

Typed file downloads and directory extraction write temporary sibling files before
replacement, so interrupted copies preserve existing content. Replacing existing files requires the provider's atomic overwrite-move
operation. The system and in-memory providers support it. Other custom providers
that cannot replace files atomically report `NotSupportedException`, preserving the
existing destination instead of deleting it before a replacement is ready.

Directory publishing still uses a temporary system file to spool the ZIP archive;
its source and destination contents use their respective path providers.

Credentials use the AWS SDK credential chain unless `AccessKey` and `SecretKey` are both set. Configure the optional
service URL when targeting an S3-compatible provider, for example `options => options with { ServiceUrl = new Uri("http://localhost:9000") }`. The URL must be absolute and use HTTP or HTTPS. Options are validated when the pipeline is built; a missing
`BucketName` fails fast.

### S3StorageOptions

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `BucketName` | `string` | `""` | Bucket to store objects in. **Required.** |
| `ServiceUrl` | `Uri?` | `null` | Endpoint for S3-compatible providers (R2, B2, MinIO). Omit for AWS S3. |
| `AccessKey` / `SecretKey` | `string?` | `null` | Explicit credentials; set both or neither. |
| `Region` | `string` | `"us-east-1"` | AWS region. |
| `ForcePathStyle` | `bool` | `false` | Path-style addressing, required by MinIO and some providers. |
| `KeyPrefix` | `string` | `"modpipe"` | Prefix for every object key. Artifacts use `{KeyPrefix}/artifacts/{RunId}/...` and module cache entries `{KeyPrefix}/module-cache/...`. |
| `MultipartPartSizeBytes` | `int` | 16 MB | Part size for large uploads (5 MB to 1 GB). Content larger than one part uses a multipart upload, so objects are not capped at 5 GB. |
| `SetLifecycleRule` | `bool` | `false` | Check an externally provisioned artifact lifecycle rule at startup and warn if it is missing or differs. Never writes bucket configuration. |
| `TimeToLive` | `TimeSpan` | 1 day | Expected artifact lifetime checked when `SetLifecycleRule` is enabled, rounded up to whole days (minimum one day). [Run data cleanup](../distributed/configuration.md#run-data-cleanup) deletes a finished run's artifacts earlier. |

### Provision lifecycle rules outside pipeline startup

Manage the bucket's complete lifecycle configuration through one infrastructure owner, such as your Terraform or
CloudFormation deployment. Pipeline startup never creates, updates, or deletes lifecycle rules, even when
`SetLifecycleRule = true`. Concurrent processes and independent runs with different prefixes therefore cannot
overwrite each other's rules or admin rules. Infrastructure deployments must coordinate their own configuration
changes: S3 replaces the entire lifecycle configuration on each write.

Provision an enabled rule for each artifact prefix. For the default `KeyPrefix = "modpipe"` and one-day
`TimeToLive`, include this rule in the bucket's lifecycle configuration alongside all other required rules:

```json
{
  "Rules": [
    {
      "ID": "modpipe-artifact-expiration:modpipe/artifacts/",
      "Status": "Enabled",
      "Filter": { "Prefix": "modpipe/artifacts/" },
      "Expiration": { "Days": 1 },
      "AbortIncompleteMultipartUpload": { "DaysAfterInitiation": 1 }
    }
  ]
}
```

Replace both occurrences of `modpipe/artifacts/` with your normalized artifact prefix: trim leading and trailing
slashes from `KeyPrefix`, then append `/artifacts/`. Set `Expiration.Days` to `TimeToLive` rounded up to whole days,
with a minimum of one day. The rule ID must be `modpipe-artifact-expiration:` followed by that prefix for the
optional startup check to recognize it. This rule covers artifacts; provision module-cache retention separately.
In versioned buckets, also configure noncurrent-version expiration when old data must be removed.

Despite its historical name, `SetLifecycleRule = true` now only reads the configuration and checks the rule's ID,
enabled status, exact prefix filter, expiration days, and one-day multipart-abort action. It requires only
`s3:GetLifecycleConfiguration`; the pipeline no longer needs `s3:PutLifecycleConfiguration`. A missing or different
rule, unsupported lifecycle API, or denied read logs a warning and allows startup to continue. Cancellation still
stops startup. This is a startup snapshot, not ongoing monitoring or a guarantee that artifacts will expire;
external changes and provider behavior remain the infrastructure owner's responsibility.

With the default `SetLifecycleRule = false`, startup makes no lifecycle API requests. Externally provisioned rules
still apply. Changing `TimeToLive` never changes the bucket policy. When migrating from automatic lifecycle writes,
keep any existing rules under infrastructure management and provision missing rules before starting pipelines.

[Run data cleanup](../distributed/configuration.md#run-data-cleanup) runs on the master when a distributed run ends.
It lists every object version under `{KeyPrefix}/artifacts/{RunId}/` and deletes each one, so the master's credentials
need these permissions in addition to the ones used for uploads and downloads:

| Permission | Used for |
|------------|----------|
| `s3:ListBucketVersions` | Listing the run's object versions and delete markers. |
| `s3:ListBucket` | Listing current objects during fallback cleanup. |
| `s3:DeleteObject` | Deleting objects in an unversioned bucket or deleting current objects during fallback cleanup in a versioned bucket. |
| `s3:DeleteObjectVersion` | Deleting specific versions and delete markers in a versioned bucket. |

If `ListObjectVersions` returns HTTP `400 Bad Request`, `403 Forbidden`, `405 Method Not Allowed`, or
`501 Not Implemented`, cleanup falls back to listing current objects with `ListObjectsV2` and deleting them
without version IDs. This requires `s3:ListBucket` and `s3:DeleteObject` in both unversioned and versioned buckets.
This supports providers without a versioning API and credentials that can list objects but lack
`s3:ListBucketVersions`. The fallback logs an information message. In a versioned bucket, it leaves old versions
and existing delete markers behind; deleting current objects only adds delete markers, so artifact data remains
recoverable. Use version-listing and version-deletion permissions or a lifecycle rule that expires noncurrent
versions when that data must be removed.

Other listing errors and deletion failures do not trigger the fallback. Cleanup logs a warning when it fails;
remaining artifacts stay until a suitable lifecycle rule expires them. Set
`DistributedOptions.RunCleanup` to `Never` to skip cleanup when the master's credentials cannot delete.

A failed multipart upload is aborted on a best-effort basis. If the abort also fails, the uploaded parts stay in the
bucket and are billed until a lifecycle rule with an `AbortIncompleteMultipartUpload` action removes them. Include that
action in any rule you configure yourself, including one covering `{KeyPrefix}/module-cache/`.

Backend-independent artifact settings, such as `ArtifactCompressionLevel`, are configured once through `DistributedOptions`:

```csharp
builder.ConfigureDistributedOptions(options => options with { ArtifactCompressionLevel = CompressionLevel.Optimal });
```

`AddS3DistributedArtifactStore` accepts either a `Func<S3StorageOptions, S3StorageOptions>` transform or an
`IConfigurationSection`. `AddS3ModuleCache` has the four overloads described below.

## Module caching

Use the same package as a shareable, cross-run module cache. The cache has its own `S3StorageOptions`, so it can use
a different bucket from the artifact store:

```csharp
builder.AddS3ModuleCache(options => options with
{
    BucketName = "pipeline-cache",
    Region = "eu-west-2",
});
```

`AddS3ModuleCache` has four overloads, with these arguments after the builder:

| Arguments | Configuration |
|-----------|---------------|
| `Func<S3StorageOptions, S3StorageOptions> configure` | S3 options transform. |
| `Func<S3StorageOptions, S3StorageOptions> configure, Func<ModuleCacheOptions, ModuleCacheOptions>? configureCache` | S3 and module-cache options transforms. |
| `IConfigurationSection section` | S3 options bound from configuration. |
| `IConfigurationSection section, Func<ModuleCacheOptions, ModuleCacheOptions>? configureCache` | S3 options binding and a module-cache options transform. |

The overloads without `configureCache` use the default module-cache settings.

See [Cache Module Results](../how-to/module-caching.md) for input declarations, artifact restoration, and fingerprint configuration.
