using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connectedvmware")]
public class AzConnectedvmwareVm
{
    public AzConnectedvmwareVm(
        AzConnectedvmwareVmDisk disk,
        AzConnectedvmwareVmExtension extension,
        AzConnectedvmwareVmGuestAgent guestAgent,
        AzConnectedvmwareVmNic nic,
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

    public AzConnectedvmwareVmDisk Disk { get; }

    public AzConnectedvmwareVmExtension Extension { get; }

    public AzConnectedvmwareVmGuestAgent GuestAgent { get; }

    public AzConnectedvmwareVmNic Nic { get; }

    public async Task<CommandResult> Create(AzConnectedvmwareVmCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzConnectedvmwareVmDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConnectedvmwareVmDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzConnectedvmwareVmListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConnectedvmwareVmListOptions(), token);
    }

    public async Task<CommandResult> Restart(AzConnectedvmwareVmRestartOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConnectedvmwareVmRestartOptions(), token);
    }

    public async Task<CommandResult> Show(AzConnectedvmwareVmShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConnectedvmwareVmShowOptions(), token);
    }

    public async Task<CommandResult> Start(AzConnectedvmwareVmStartOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConnectedvmwareVmStartOptions(), token);
    }

    public async Task<CommandResult> Stop(AzConnectedvmwareVmStopOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConnectedvmwareVmStopOptions(), token);
    }

    public async Task<CommandResult> Update(AzConnectedvmwareVmUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConnectedvmwareVmUpdateOptions(), token);
    }
}