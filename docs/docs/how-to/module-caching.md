---
title: Cache Module Results
---

# Cache Module Results

Fingerprint-based module caching skips work when its declared inputs have not changed. A cache entry contains the typed module result and files declared with `ProducesArtifact`, so a hit can restore outputs before dependent modules start.

## Enable a local cache

Register the filesystem backend once:

```csharp
using ModularPipelines.Caching;
using ModularPipelines.Extensions;

builder.AddModuleCache<FileSystemModuleCache>();
```

Then declare every file input that can affect a module:

```csharp
[CacheInputs("src/**/*.cs", "*.csproj")]
[ProducesArtifact("application", "artifacts/publish/**/*")]
public sealed class BuildModule : Module<BuildOutput>
{
    protected override Task<BuildOutput?> ExecuteAsync(
        IModuleContext context,
        CancellationToken cancellationToken)
    {
        // Build the application.
    }
}
```

Patterns are relative to `ModuleCacheOptions.WorkingDirectory`. They support `*`, `?`, and recursive `**` wildcards.

The fingerprint contains:

- the module type and assembly version;
- the content and relative path of every declared input;
- explicit key parts and declared environment variable values;
- normalized results from all direct, selector-based, and dynamic dependencies.

A hit has `Status.UsedHistory`. Cache lookup or storage failures are logged and the module executes normally.

## Include configuration

File contents do not represent every input. Add configuration or tool versions explicitly:

```csharp
protected override ModuleConfiguration Configure() =>
    ModuleConfiguration.Create()
        .WithCacheKeyPart($"configuration={configurationName}")
        .WithCacheKeyPart($"sdk={sdkVersion}")
        .WithCacheEnvironmentVariable("TARGET_RUNTIME")
        .Build();
```

Changing any key part or declared environment value invalidates the entry.

## Configure limits and locations

```csharp
builder.AddModuleCache<FileSystemModuleCache>(options =>
{
    options.WorkingDirectory = repositoryRoot;
    options.CacheDirectory = Path.Combine(repositoryRoot, ".cache", "modules");
    options.MaximumInputFiles = 50_000;
    options.MaximumHashConcurrency = 8;
});
```

The file limit prevents unexpectedly broad globs. File hashes use a persistent modification-time and size index; changed files are hashed concurrently.

## Share entries through S3 or Redis

The S3 and Redis packages provide cross-run cache backends. Their cache namespaces do not use distributed pipeline run identifiers, so concurrent runs can safely share entries.

```csharp
builder.AddS3ModuleCache(options =>
{
    options.BucketName = "pipeline-artifacts";
    options.Region = "eu-west-2";
});
```

Or use Redis:

```csharp
builder.AddRedisModuleCache(
    redis => redis.ConnectionString = "localhost:6379",
    cacheEntries => cacheEntries.TimeToLiveSeconds = 86_400);
```

## Correctness rules

- Declare all file inputs. An undeclared input cannot invalidate a cache entry.
- Add key parts for arguments, configuration objects, external service versions, and other non-file inputs.
- Return dependency values that change whenever relevant upstream state changes. Only direct dependency results are fingerprinted, so constant or `Unit` results do not propagate transitive invalidation.
- Declare environment variables individually; the framework does not fingerprint the entire process environment.
- Input files are content-hashed on every fingerprint calculation. File size and timestamps are not treated as proof that content is unchanged.
- Use `ProducesArtifact` for files a cache hit must recreate.
- Do not cache modules whose result cannot be serialized to JSON.
