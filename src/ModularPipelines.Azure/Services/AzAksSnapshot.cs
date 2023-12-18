using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("aks")]
public class AzAksSnapshot
{
    public AzAksSnapshot(
        AzAksSnapshotCreate create,
        AzAksSnapshotDelete delete,
        AzAksSnapshotList list,
        AzAksSnapshotShow show,
        ICommand internalCommand
    )
    {
        CreateCommands = create;
        DeleteCommands = delete;
        ListCommands = list;
        ShowCommands = show;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzAksSnapshotCreate CreateCommands { get; }

    public AzAksSnapshotDelete DeleteCommands { get; }

    public AzAksSnapshotList ListCommands { get; }

    public AzAksSnapshotShow ShowCommands { get; }

    public async Task<CommandResult> Create(AzAksSnapshotCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzAksSnapshotDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzAksSnapshotListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzAksSnapshotShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Wait(AzAksSnapshotWaitOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}