using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzSnapshot
{
    public AzSnapshot(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzSnapshotCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzSnapshotDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSnapshotDeleteOptions(), token);
    }

    public async Task<CommandResult> GrantAccess(AzSnapshotGrantAccessOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzSnapshotListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSnapshotListOptions(), token);
    }

    public async Task<CommandResult> RevokeAccess(AzSnapshotRevokeAccessOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSnapshotRevokeAccessOptions(), token);
    }

    public async Task<CommandResult> Show(AzSnapshotShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSnapshotShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzSnapshotUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSnapshotUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzSnapshotWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSnapshotWaitOptions(), token);
    }
}