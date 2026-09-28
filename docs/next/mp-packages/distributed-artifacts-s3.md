# Distributed S3 Artifacts Package

`ModularPipelines.Distributed.Artifacts.S3` stores distributed pipeline artifacts in AWS S3 or an S3-compatible service such as Cloudflare R2, Backblaze B2, or MinIO.

## Installation[​](#installation "Direct link to Installation")

```
dotnet add package ModularPipelines.Distributed.Artifacts.S3
```

## Configuration[​](#configuration "Direct link to Configuration")

Register the artifact store after enabling distributed mode:

```
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

Modules publish and download artifacts through the context property. Cancellation tokens are optional, and the typed download overload identifies the producer without a string module name:

```
await context.Artifacts.PublishFileAsync("package", packagePath);

await context.Artifacts.DownloadAsync<BuildModule>(

    "package",

    Path.Combine(context.Environment.WorkingDirectory.Path, "package.zip"));
```

Credentials use the AWS SDK credential chain unless `AccessKey` and `SecretKey` are both set. Configure the optional service URL when targeting an S3-compatible provider. Options are validated when the pipeline is built; a missing `BucketName` fails fast.

### S3StorageOptions[​](#s3storageoptions "Direct link to S3StorageOptions")

| Property                  | Type       | Default       | Description                                                                                                                                  |
| ------------------------- | ---------- | ------------- | -------------------------------------------------------------------------------------------------------------------------------------------- |
| `BucketName`              | `string`   | `""`          | Bucket to store objects in. **Required.**                                                                                                    |
| `ServiceUrl`              | `string?`  | `null`        | Endpoint for S3-compatible providers (R2, B2, MinIO). Omit for AWS S3.                                                                       |
| `AccessKey` / `SecretKey` | `string?`  | `null`        | Explicit credentials; set both or neither.                                                                                                   |
| `Region`                  | `string`   | `"us-east-1"` | AWS region.                                                                                                                                  |
| `ForcePathStyle`          | `bool`     | `false`       | Path-style addressing, required by MinIO and some providers.                                                                                 |
| `KeyPrefix`               | `string`   | `"modpipe"`   | Prefix for every object key. Artifacts use `{KeyPrefix}/artifacts/{RunId}/...` and module cache entries `{KeyPrefix}/module-cache/...`.      |
| `MultipartPartSizeBytes`  | `int`      | 16 MB         | Part size for large uploads (5 MB to 1 GB). Content larger than one part uses a multipart upload, so objects are not capped at 5 GB.         |
| `SetLifecycleRule`        | `bool`     | `false`       | Add or update a bucket lifecycle rule that expires artifacts after `TimeToLive` and aborts their incomplete multipart uploads after one day. |
| `TimeToLive`              | `TimeSpan` | 1 day         | Artifact lifetime used by the lifecycle rule, rounded up to whole days.                                                                      |

`SetLifecycleRule` reads the bucket's lifecycle configuration and merges one rule, identified by the artifact prefix, into it; other rules are preserved. It needs the `s3:GetLifecycleConfiguration` and `s3:PutLifecycleConfiguration` permissions. If the provider does not support lifecycle configuration or access is denied, a warning is logged and artifacts simply do not expire automatically. Because every process runs this check at startup, prefer configuring the rule once in your infrastructure code and leaving `SetLifecycleRule` off.

A failed multipart upload is aborted on a best-effort basis. If the abort also fails, the uploaded parts stay in the bucket and are billed until a lifecycle rule with an `AbortIncompleteMultipartUpload` action removes them. Include that action in any rule you configure yourself, including one covering `{KeyPrefix}/module-cache/`.

Backend-independent artifact settings, such as `CompressionLevel`, are configured once through `ArtifactOptions`:

```
builder.Services.Configure<ArtifactOptions>(options => options.CompressionLevel = CompressionLevel.Optimal);
```

Both `AddS3DistributedArtifactStore` and `AddS3ModuleCache` have one `Action<S3StorageOptions>` overload and one `IConfigurationSection` overload.

## Module caching[​](#module-caching "Direct link to Module caching")

Use the same package as a shareable, cross-run module cache. The cache has its own `S3StorageOptions`, so it can use a different bucket from the artifact store:

```
builder.AddS3ModuleCache(options =>

{

    options.BucketName = "pipeline-cache";

    options.Region = "eu-west-2";

});
```

See [Cache Module Results](/ModularPipelines/docs/next/how-to/module-caching.md) for input declarations, artifact restoration, and fingerprint configuration.
