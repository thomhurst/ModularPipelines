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
using ModularPipelines.Distributed.Artifacts.S3.Extensions;
using ModularPipelines.Distributed.Extensions;

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

Credentials use the AWS SDK credential chain. Configure the optional service URL when targeting an S3-compatible provider.

## Module caching

Use the same package as a shareable, cross-run module cache:

```csharp
builder.AddS3ModuleCache(options =>
{
    options.BucketName = "pipeline-cache";
    options.Region = "eu-west-2";
});
```

See [Cache Module Results](../how-to/module-caching.md) for input declarations, artifact restoration, and fingerprint configuration.
