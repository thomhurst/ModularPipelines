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

- the module type and its assembly module version ID (MVID), unless explicitly overridden;
- the content and relative path of every declared input;
- explicit key parts and declared environment variable values;
- normalized results from all direct, selector-based, and dynamic dependencies.

A history hit has `ModuleStatus.RestoredFromHistory`; a fingerprint-cache hit has
`ModuleStatus.RestoredFromCache`. Cache lookup or storage failures are logged and the module executes normally.

Use `--no-cache` to bypass all module cache reads and writes for one command-line run. For programmatic control, set `PipelineOptions.DisableModuleCache` to `true`.

## Include configuration

File contents do not represent every input. Add configuration or tool versions explicitly:

```csharp
protected override void Configure(ModuleConfigurationBuilder module) => module
    .WithCacheKeyPart($"configuration={configurationName}")
    .WithCacheKeyPart($"sdk={sdkVersion}")
    .WithCacheEnvironmentVariable("TARGET_RUNTIME");
```

Changing any key part or declared environment value invalidates the entry.

## Stabilize the module version key

By default, the fingerprint includes the module assembly's MVID so rebuilding changed module
code invalidates cached results. Some versioning tools generate different assembly metadata for
every commit, which can also change the MVID when the module implementation is unchanged. This
can eliminate cross-commit cache hits, and changing any code in the assembly can invalidate every
cached module declared by that assembly.

Use an explicit version key only when your build changes the MVID independently of module behavior:

```csharp
protected override void Configure(ModuleConfigurationBuilder module) => module
    .WithCacheKeyPart("configuration=v1")
    .WithCacheAssemblyVersionKey("build-module-v3");
```

You must update this key whenever the module implementation changes. Reusing it after a behavior
change can restore stale results or artifacts. Cache misses log bounded fingerprint-component
diagnostics at `Debug`; user-controlled key parts, environment values, exception messages, and
skip reasons appear only as SHA-256 hashes.

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

The file limit prevents unexpectedly broad globs. Input files are content-hashed concurrently on
every fingerprint calculation, up to `MaximumHashConcurrency` files at a time.

## Transfer artifacts between modules

Declare a produced file and consume it through a required dependency:

```csharp
[ProducesArtifact("application", "artifacts/publish.zip")]
public sealed class BuildModule : Module<Unit>
{
    // Create artifacts/publish.zip.
}

[DependsOn<BuildModule>]
[ConsumesArtifact(typeof(BuildModule), "application", RestorePath = "deploy")]
public sealed class DeployModule : Module<Unit>
{
    // Read deploy/application.
}
```

Artifact transfer works for standalone and distributed pipelines. Validation requires the producer to be registered, the artifact name to match exactly, and the consumer to reach the producer through required dependencies. An optional dependency cannot guarantee that an artifact exists.

Relative produce and restore paths use `ModuleCacheOptions.WorkingDirectory`. Standalone execution uploads only artifacts needed by runnable consumers, and fails the consumer before execution when a required artifact is missing.

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
