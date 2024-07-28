---
title: Module History
---

# Module History

## What is it?
If a repository has been set up, then when a module finishes without throwing an exception, it will attempt to save its result in the repository.

This can be used later on if you want to re-run the pipeline but skip certain categories or modules.

If a module is skipped, but it has a result available from a previous run, then it'll be reconstructed from the historical result, and it'll act as if it was successfully run.

It's recommended to store and retrieve results using the Git commit SHA, as then you'll only be using the results from that previous commit's run. And other commits remain unaffected. You have access to a `context` object that gives you access to things like the git information.

## Example Repository Class using Azure Blobs

```csharp
await PipelineHostBuilder.Create()
    .AddModule<Module1>()
    .AddModule<Module2>()
    .AddModule<Module3>()
    .AddResultsRepository<MyModuleRepository>()
    .ExecutePipelineAsync();

```

```csharp
public class MyModuleRepository : IModuleResultRepository
{
    private readonly BlobContainerClient _blobContainerClient;

    public MyModuleRepository(BlobContainerClient blobContainerClient)
    {
        _blobContainerClient = blobContainerClient;
    }
    
    public async Task SaveResultAsync<T>(ModuleBase module, ModuleResult<T> moduleResult, IPipelineHookContext pipelineContext)
    {
        var commit = pipelineContext.Git().Information.LastCommitSha;
        await _blobContainerClient.UploadBlobAsync(module.GetType().FullName + commit, new BinaryData(JsonSerializer.Serialize(moduleResult)));
    }

    public async Task<ModuleResult<T>?> GetResultAsync<T>(ModuleBase module, IPipelineHookContext pipelineContext)
    {
        var commit = pipelineContext.Git().Information.LastCommitSha;
        
        var blobContent = await _blobContainerClient.GetBlobClient(module.GetType().FullName + commit).DownloadContentAsync();

        return JsonSerializer.Deserialize<ModuleResult<T>>(blobContent.Value.Content.ToString());
    }
}
```