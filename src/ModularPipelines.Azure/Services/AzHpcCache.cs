using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzHpcCache
{
    public AzHpcCache(
        AzHpcCacheBlobStorageTarget blobStorageTarget,
        AzHpcCacheNfsStorageTarget nfsStorageTarget,
        AzHpcCacheSkus skus,
        AzHpcCacheStorageTarget storageTarget,
        AzHpcCacheUsageModel usageModel,
        ICommand internalCommand
    )
    {
        BlobStorageTarget = blobStorageTarget;
        NfsStorageTarget = nfsStorageTarget;
        Skus = skus;
        StorageTarget = storageTarget;
        UsageModel = usageModel;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzHpcCacheBlobStorageTarget BlobStorageTarget { get; }

    public AzHpcCacheNfsStorageTarget NfsStorageTarget { get; }

    public AzHpcCacheSkus Skus { get; }

    public AzHpcCacheStorageTarget StorageTarget { get; }

    public AzHpcCacheUsageModel UsageModel { get; }

    public async Task<CommandResult> Create(AzHpcCacheCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzHpcCacheDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Flush(AzHpcCacheFlushOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzHpcCacheListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzHpcCacheShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Start(AzHpcCacheStartOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Stop(AzHpcCacheStopOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(AzHpcCacheUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpgradeFirmware(AzHpcCacheUpgradeFirmwareOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Wait(AzHpcCacheWaitOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}