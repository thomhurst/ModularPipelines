using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
public class AzAppconfig
{
    public AzAppconfig(
        AzAppconfigCredential credential,
        AzAppconfigFeature feature,
        AzAppconfigIdentity identity,
        AzAppconfigKv kv,
        AzAppconfigReplica replica,
        AzAppconfigRevision revision,
        AzAppconfigSnapshot snapshot,
        ICommand internalCommand
    )
    {
        Credential = credential;
        Feature = feature;
        Identity = identity;
        Kv = kv;
        Replica = replica;
        Revision = revision;
        Snapshot = snapshot;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzAppconfigCredential Credential { get; }

    public AzAppconfigFeature Feature { get; }

    public AzAppconfigIdentity Identity { get; }

    public AzAppconfigKv Kv { get; }

    public AzAppconfigReplica Replica { get; }

    public AzAppconfigRevision Revision { get; }

    public AzAppconfigSnapshot Snapshot { get; }

    public async Task<CommandResult> Create(AzAppconfigCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzAppconfigDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzAppconfigListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListDeleted(AzAppconfigListDeletedOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Purge(AzAppconfigPurgeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Recover(AzAppconfigRecoverOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzAppconfigShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ShowDeleted(AzAppconfigShowDeletedOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(AzAppconfigUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}