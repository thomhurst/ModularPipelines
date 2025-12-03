using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("scvmm")]
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

    public async Task<CommandResult> Delete(AzScvmmVmDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzScvmmVmDeleteOptions(), token);
    }

    public async Task<CommandResult> DeleteCheckpoint(AzScvmmVmDeleteCheckpointOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Restart(AzScvmmVmRestartOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzScvmmVmRestartOptions(), token);
    }

    public async Task<CommandResult> RestoreCheckpoint(AzScvmmVmRestoreCheckpointOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzScvmmVmShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzScvmmVmShowOptions(), token);
    }

    public async Task<CommandResult> Start(AzScvmmVmStartOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzScvmmVmStartOptions(), token);
    }

    public async Task<CommandResult> Stop(AzScvmmVmStopOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzScvmmVmStopOptions(), token);
    }

    public async Task<CommandResult> Update(AzScvmmVmUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzScvmmVmUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzScvmmVmWaitOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}