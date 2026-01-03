using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("urestackhci")]
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
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Delete(AzAzurestackhciVirtualmachineDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAzurestackhciVirtualmachineDeleteOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> List(AzAzurestackhciVirtualmachineListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAzurestackhciVirtualmachineListOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Restart(AzAzurestackhciVirtualmachineRestartOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAzurestackhciVirtualmachineRestartOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzAzurestackhciVirtualmachineShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAzurestackhciVirtualmachineShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Start(AzAzurestackhciVirtualmachineStartOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAzurestackhciVirtualmachineStartOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Stop(AzAzurestackhciVirtualmachineStopOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAzurestackhciVirtualmachineStopOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Update(AzAzurestackhciVirtualmachineUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAzurestackhciVirtualmachineUpdateOptions(), cancellationToken: token);
    }
}