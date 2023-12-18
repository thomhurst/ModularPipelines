using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("scvmm")]
public class AzScvmmVm
{
    public AzScvmmVm(
        AzScvmmVmDisk disk,
        AzScvmmVmExtension extension,
        AzScvmmVmGuestAgent guestAgent,
        AzScvmmVmNic nic,
        ICommand internalCommand
    )
    {
        Disk = disk;
        Extension = extension;
        GuestAgent = guestAgent;
        Nic = nic;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzScvmmVmDisk Disk { get; }

    public AzScvmmVmExtension Extension { get; }

    public AzScvmmVmGuestAgent GuestAgent { get; }

    public AzScvmmVmNic Nic { get; }

    public async Task<CommandResult> Create(AzScvmmVmCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateCheckpoint(AzScvmmVmCreateCheckpointOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzScvmmVmDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteCheckpoint(AzScvmmVmDeleteCheckpointOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzScvmmVmListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Restart(AzScvmmVmRestartOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RestoreCheckpoint(AzScvmmVmRestoreCheckpointOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzScvmmVmShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Start(AzScvmmVmStartOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Stop(AzScvmmVmStopOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(AzScvmmVmUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Wait(AzScvmmVmWaitOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}

