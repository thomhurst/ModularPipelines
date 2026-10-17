---
title: Module History
---

# Module History

## What is it?
If a repository has been set up, the pipeline attempts to save successful module results and
failure results whose failures are ignored (`FailureIgnored`). An ignored failure still contains
an exception and has no successful value.

This can be used later on if you want to re-run the pipeline but skip certain categories or modules.

If a module is skipped and a historical result is available, the pipeline restores that result
with status `RestoredFromHistory`. Restoration changes the status, not the result variant:
a stored failure remains a failure, and reading its `.Value` throws. Dependent modules must
inspect the outcome before consuming the value; the restored status alone does not prove success.

For example, when a dependency returns `ModuleResult<string>`, handle each variant:

```csharp
switch (result)
{
    case ModuleResult<string>.Success { Value: var value }:
        // Consume value.
        break;
    case ModuleResult<string>.Failure { Exception: var exception }:
        // Handle the stored failure.
        break;
    case ModuleResult<string>.Skipped { Decision: var decision }:
        // Handle the skipped outcome.
        break;
}
```

Alternatively, use `TryGetValue(out var value)` to check for a successful value without throwing.
`ValueOrDefault` also avoids throwing, but cannot distinguish a failure from a successful default value.

It's recommended to store and retrieve results using the Git commit SHA, as then you'll only be using the results from that previous commit's run. And other commits remain unaffected. You have access to a `context` object that gives you access to things like the git information.

## Example Repository Class using Azure Blobs

```csharp
var builder = Pipeline.CreateBuilder(args);

builder
    .AddModule<Module1>()
    .AddModule<Module2>()
    .AddModule<Module3>();

builder.AddResultsRepository<MyModuleRepository>();

await builder.RunAsync();
```

Result history resolves type-erased module results at runtime and is not supported for
trimmed or Native AOT applications. `AddResultsRepository<TRepository>()` emits the
corresponding trim and dynamic-code warnings.

The example below keys each result by commit SHA, so it requires a Git checkout.
Outside one, `GetRequiredInfoAsync` throws, so the example neither stores nor restores
history there. To keep history outside Git, call `GetInfoAsync` instead and choose a
fallback key when it returns `null`.

```csharp
public class MyModuleRepository : IModuleResultRepository
{
    private readonly BlobContainerClient _blobContainerClient;

    public MyModuleRepository(BlobContainerClient blobContainerClient)
    {
        _blobContainerClient = blobContainerClient;
    }
    
    public async Task SaveResultAsync<T>(
        Module<T> module,
        ModuleResult<T> moduleResult,
        IPipelineContext pipelineContext,
        CancellationToken cancellationToken = default)
    {
        var repositoryInfo = await pipelineContext.Tools.Git.Information.GetRequiredInfoAsync(cancellationToken);
        var commit = repositoryInfo.LastCommitSha;
        await _blobContainerClient.UploadBlobAsync(
            module.GetType().FullName + commit,
            new BinaryData(JsonSerializer.Serialize(moduleResult)),
            cancellationToken);
    }

    public async Task<ModuleResult<T>?> GetResultAsync<T>(
        Module<T> module,
        IPipelineContext pipelineContext,
        CancellationToken cancellationToken = default)
    {
        var repositoryInfo = await pipelineContext.Tools.Git.Information.GetRequiredInfoAsync(cancellationToken);
        var commit = repositoryInfo.LastCommitSha;

        var blobContent = await _blobContainerClient
            .GetBlobClient(module.GetType().FullName + commit)
            .DownloadContentAsync(cancellationToken);

        return JsonSerializer.Deserialize<ModuleResult<T>>(blobContent.Value.Content.ToString());
    }
}
```

## Behavior

- Restored results have the `RestoredFromHistory` status and retain their success, failure, or
  skipped variant. Dependent modules can inspect them, but only a success variant has a value.
- Repository failures are best-effort: exceptions from `SaveResultAsync` and `GetResultAsync` are
  logged, and the module runs (or is skipped) as if no result was stored.
- `IsEnabled` defaults to `true`; return `false` to switch the repository off without removing it.
- Result history is separate from [module caching](module-caching.md), which restores results by
  input fingerprint, and from the [time estimator](time-estimator.md), which stores durations.
