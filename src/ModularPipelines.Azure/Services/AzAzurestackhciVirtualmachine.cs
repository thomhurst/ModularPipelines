using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("urestackhci")]
public class AzAzurestackhciVirtualmachine
{
    public AzAzurestackhciVirtualmachine(
        AzAzurestackhciVirtualmachineExtension extension,
        AzAzurestackhciVirtualmachineVnic vnic,
        ICommand internalCommand
    )
    {
        Extension = extension;
        Vnic = vnic;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzAzurestackhciVirtualmachineExtension Extension { get; }

    public AzAzurestackhciVirtualmachineVnic Vnic { get; }

    public async Task<CommandResult> Create(AzAzurestackhciVirtualmachineCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzAzurestackhciVirtualmachineDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAzurestackhciVirtualmachineDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzAzurestackhciVirtualmachineListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAzurestackhciVirtualmachineListOptions(), token);
    }

    public async Task<CommandResult> Restart(AzAzurestackhciVirtualmachineRestartOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAzurestackhciVirtualmachineRestartOptions(), token);
    }

    public async Task<CommandResult> Show(AzAzurestackhciVirtualmachineShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAzurestackhciVirtualmachineShowOptions(), token);
    }

    public async Task<CommandResult> Start(AzAzurestackhciVirtualmachineStartOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAzurestackhciVirtualmachineStartOptions(), token);
    }

    public async Task<CommandResult> Stop(AzAzurestackhciVirtualmachineStopOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAzurestackhciVirtualmachineStopOptions(), token);
    }

    public async Task<CommandResult> Update(AzAzurestackhciVirtualmachineUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAzurestackhciVirtualmachineUpdateOptions(), token);
    }
}