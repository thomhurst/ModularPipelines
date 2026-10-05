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

builder.AddDistributedMode(options => options.TotalInstances = 2);
builder.AddS3DistributedArtifactStore(options =>
{
    options.BucketName = "pipeline-artifacts";
    options.Region = "eu-west-2";
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

Replacing existing directory entries requires the provider's atomic overwrite-move
operation. The system and in-memory providers support it. Other custom providers
that cannot replace files atomically report `NotSupportedException`, preserving the
existing destination instead of deleting it before a replacement is ready.

Directory publishing still uses a temporary system file to spool the ZIP archive;
its source and destination contents use their respective path providers.

Credentials use the AWS SDK credential chain unless `AccessKey` and `SecretKey` are both set. Configure the optional
service URL when targeting an S3-compatible provider. Options are validated when the pipeline is built; a missing
`BucketName` fails fast.

### S3StorageOptions

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `BucketName` | `string` | `""` | Bucket to store objects in. **Required.** |
| `ServiceUrl` | `string?` | `null` | Endpoint for S3-compatible providers (R2, B2, MinIO). Omit for AWS S3. |
| `AccessKey` / `SecretKey` | `string?` | `null` | Explicit credentials; set both or neither. |
| `Region` | `string` | `"us-east-1"` | AWS region. |
| `ForcePathStyle` | `bool` | `false` | Path-style addressing, required by MinIO and some providers. |
| `KeyPrefix` | `string` | `"modpipe"` | Prefix for every object key. Artifacts use `{KeyPrefix}/artifacts/{RunId}/...` and module cache entries `{KeyPrefix}/module-cache/...`. |
| `MultipartPartSizeBytes` | `int` | 16 MB | Part size for large uploads (5 MB to 1 GB). Content larger than one part uses a multipart upload, so objects are not capped at 5 GB. |
| `SetLifecycleRule` | `bool` | `false` | Add or update a bucket lifecycle rule that expires artifacts after `TimeToLive` and aborts their incomplete multipart uploads after one day. |
| `TimeToLive` | `TimeSpan` | 1 day | Artifact lifetime used by the lifecycle rule, rounded up to whole days. |

`SetLifecycleRule` reads the bucket's lifecycle configuration and merges one rule, identified by the artifact prefix,
into it; other rules are preserved. It needs the `s3:GetLifecycleConfiguration` and `s3:PutLifecycleConfiguration`
permissions. If the provider does not support lifecycle configuration or access is denied, a warning is logged and
artifacts simply do not expire automatically. Because every process runs this check at startup, prefer configuring
the rule once in your infrastructure code and leaving `SetLifecycleRule` off.

A failed multipart upload is aborted on a best-effort basis. If the abort also fails, the uploaded parts stay in the
bucket and are billed until a lifecycle rule with an `AbortIncompleteMultipartUpload` action removes them. Include that
action in any rule you configure yourself, including one covering `{KeyPrefix}/module-cache/`.

Backend-independent artifact settings, such as `ArtifactCompressionLevel`, are configured once through `DistributedOptions`:

```csharp
builder.Services.Configure<DistributedOptions>(options => options.ArtifactCompressionLevel = CompressionLevel.Optimal);
```

Both `AddS3DistributedArtifactStore` and `AddS3ModuleCache` have one `Action<S3StorageOptions>` overload and one
`IConfigurationSection` overload.

## Module caching

Use the same package as a shareable, cross-run module cache. The cache has its own `S3StorageOptions`, so it can use
a different bucket from the artifact store:

```csharp
builder.AddS3ModuleCache(options =>
{
    options.BucketName = "pipeline-cache";
    options.Region = "eu-west-2";
});
```

See [Cache Module Results](../how-to/module-caching.md) for input declarations, artifact restoration, and fingerprint configuration.
