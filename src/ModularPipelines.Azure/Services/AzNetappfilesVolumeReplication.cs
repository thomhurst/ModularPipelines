using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("netappfiles", "volume")]
public class AzNetappfilesVolumeReplication
{
    public AzNetappfilesVolumeReplication(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Approve(AzNetappfilesVolumeReplicationApproveOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzNetappfilesVolumeReplicationListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ReInitialize(AzNetappfilesVolumeReplicationReInitializeOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetappfilesVolumeReplicationReInitializeOptions(), token);
    }

    public async Task<CommandResult> Remove(AzNetappfilesVolumeReplicationRemoveOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetappfilesVolumeReplicationRemoveOptions(), token);
    }

    public async Task<CommandResult> Resume(AzNetappfilesVolumeReplicationResumeOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetappfilesVolumeReplicationResumeOptions(), token);
    }

    public async Task<CommandResult> Status(AzNetappfilesVolumeReplicationStatusOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetappfilesVolumeReplicationStatusOptions(), token);
    }

    public async Task<CommandResult> Suspend(AzNetappfilesVolumeReplicationSuspendOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetappfilesVolumeReplicationSuspendOptions(), token);
    }

    public async Task<CommandResult> Wait(AzNetappfilesVolumeReplicationWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetappfilesVolumeReplicationWaitOptions(), token);
    }
}