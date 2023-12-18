using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage")]
public class AzStorageBlob
{
    public AzStorageBlob(
        AzStorageBlobAccess access,
        AzStorageBlobCopy copy,
        AzStorageBlobDelete delete,
        AzStorageBlobDirectory directory,
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
        Access = access;
        Copy = copy;
        DeleteCommands = delete;
        Directory = directory;
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

    public AzStorageBlobAccess Access { get; }

    public AzStorageBlobCopy Copy { get; }

    public AzStorageBlobDelete DeleteCommands { get; }

    public AzStorageBlobDirectory Directory { get; }

    public AzStorageBlobGenerateSas GenerateSasCommands { get; }

    public AzStorageBlobImmutabilityPolicy ImmutabilityPolicy { get; }

    public AzStorageBlobIncrementalCopy IncrementalCopy { get; }

    public AzStorageBlobLease Lease { get; }

    public AzStorageBlobMetadata Metadata { get; }

    public AzStorageBlobServiceProperties ServiceProperties { get; }

    public AzStorageBlobSetTier SetTierCommands { get; }

    public AzStorageBlobShow ShowCommands { get; }

    public AzStorageBlobTag Tag { get; }

    public async Task<CommandResult> Delete(AzStorageBlobDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteBatch(AzStorageBlobDeleteBatchOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Download(AzStorageBlobDownloadOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DownloadBatch(AzStorageBlobDownloadBatchOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Exists(AzStorageBlobExistsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Filter(AzStorageBlobFilterOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GenerateSas(AzStorageBlobGenerateSasOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzStorageBlobListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Move(AzStorageBlobMoveOptions options, CancellationToken token = default)
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

    public async Task<CommandResult> Show(AzStorageBlobShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Snapshot(AzStorageBlobSnapshotOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Sync(AzStorageBlobSyncOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Undelete(AzStorageBlobUndeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(AzStorageBlobUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Upload(AzStorageBlobUploadOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
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