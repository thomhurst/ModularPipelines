using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("aks", "nodepool")]
public class AzAksNodepoolSnapshot
{
    public AzAksNodepoolSnapshot(
        AzAksNodepoolSnapshotCreate create,
        AzAksNodepoolSnapshotDelete delete,
        AzAksNodepoolSnapshotList list,
        AzAksNodepoolSnapshotShow show,
        AzAksNodepoolSnapshotUpdate update,
        ICommand internalCommand
    )
    {
        CreateCommands = create;
        DeleteCommands = delete;
        ListCommands = list;
        ShowCommands = show;
        UpdateCommands = update;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzAksNodepoolSnapshotCreate CreateCommands { get; }

    public AzAksNodepoolSnapshotDelete DeleteCommands { get; }

    public AzAksNodepoolSnapshotList ListCommands { get; }

    public AzAksNodepoolSnapshotShow ShowCommands { get; }

    public AzAksNodepoolSnapshotUpdate UpdateCommands { get; }

    public async Task<CommandResult> Create(AzAksNodepoolSnapshotCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzAksNodepoolSnapshotDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzAksNodepoolSnapshotListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAksNodepoolSnapshotListOptions(), token);
    }

    public async Task<CommandResult> Show(AzAksNodepoolSnapshotShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(AzAksNodepoolSnapshotUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Wait(AzAksNodepoolSnapshotWaitOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}