using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("storage")]
public class AzStorageBlob
{
    public AzStorageBlob(
        AzStorageBlobCopy copy,
        AzStorageBlobDelete delete,
        AzStorageBlobGenerateSas generateSas,
        AzStorageBlobImmutabilityPolicy immutabilityPolicy,
        AzStorageBlobIncrementalCopy incrementalCopy,
        AzStorageBlobLease lease,
        AzStorageBlobMetadata metadata,
        AzStorageBlobServiceProperties serviceProperties,
        AzStorageBlobSetTier setTier,
        AzStorageBlobShow show,
        AzStorageBlobTag tag,
        ICommand internalCommand
    )
    {
        Copy = copy;
        DeleteCommands = delete;
        GenerateSasCommands = generateSas;
        ImmutabilityPolicy = immutabilityPolicy;
        IncrementalCopy = incrementalCopy;
        Lease = lease;
        Metadata = metadata;
        ServiceProperties = serviceProperties;
        SetTierCommands = setTier;
        ShowCommands = show;
        Tag = tag;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzStorageBlobCopy Copy { get; }

    public AzStorageBlobDelete DeleteCommands { get; }

    public AzStorageBlobGenerateSas GenerateSasCommands { get; }

    public AzStorageBlobImmutabilityPolicy ImmutabilityPolicy { get; }

    public AzStorageBlobIncrementalCopy IncrementalCopy { get; }

    public AzStorageBlobLease Lease { get; }

    public AzStorageBlobMetadata Metadata { get; }

    public AzStorageBlobServiceProperties ServiceProperties { get; }

    public AzStorageBlobSetTier SetTierCommands { get; }

    public AzStorageBlobShow ShowCommands { get; }

    public AzStorageBlobTag Tag { get; }

    public async Task<CommandResult> Delete(AzStorageBlobDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStorageBlobDeleteOptions(), token);
    }

    public async Task<CommandResult> DeleteBatch(AzStorageBlobDeleteBatchOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Download(AzStorageBlobDownloadOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStorageBlobDownloadOptions(), token);
    }

    public async Task<CommandResult> DownloadBatch(AzStorageBlobDownloadBatchOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Exists(AzStorageBlobExistsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStorageBlobExistsOptions(), token);
    }

    public async Task<CommandResult> Filter(AzStorageBlobFilterOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GenerateSas(AzStorageBlobGenerateSasOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStorageBlobGenerateSasOptions(), token);
    }

    public async Task<CommandResult> List(AzStorageBlobListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Query(AzStorageBlobQueryOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Restore(AzStorageBlobRestoreOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Rewrite(AzStorageBlobRewriteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SetLegalHold(AzStorageBlobSetLegalHoldOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SetTier(AzStorageBlobSetTierOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzStorageBlobShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStorageBlobShowOptions(), token);
    }

    public async Task<CommandResult> Snapshot(AzStorageBlobSnapshotOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStorageBlobSnapshotOptions(), token);
    }

    public async Task<CommandResult> Sync(AzStorageBlobSyncOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Undelete(AzStorageBlobUndeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStorageBlobUndeleteOptions(), token);
    }

    public async Task<CommandResult> Update(AzStorageBlobUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Upload(AzStorageBlobUploadOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStorageBlobUploadOptions(), token);
    }

    public async Task<CommandResult> UploadBatch(AzStorageBlobUploadBatchOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Url(AzStorageBlobUrlOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}