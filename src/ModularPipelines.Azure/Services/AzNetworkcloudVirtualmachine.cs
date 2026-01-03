using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("networkcloud")]
public class AzNetworkcloudVirtualmachine
{
    public AzNetworkcloudVirtualmachine(
        AzNetworkcloudVirtualmachineConsole console,
        ICommand internalCommand
    )
    {
        Console = console;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzNetworkcloudVirtualmachineConsole Console { get; }

    public async Task<CommandResult> Create(AzNetworkcloudVirtualmachineCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Delete(AzNetworkcloudVirtualmachineDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkcloudVirtualmachineDeleteOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> List(AzNetworkcloudVirtualmachineListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkcloudVirtualmachineListOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> PowerOff(AzNetworkcloudVirtualmachinePowerOffOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkcloudVirtualmachinePowerOffOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Reimage(AzNetworkcloudVirtualmachineReimageOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkcloudVirtualmachineReimageOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Restart(AzNetworkcloudVirtualmachineRestartOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkcloudVirtualmachineRestartOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzNetworkcloudVirtualmachineShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkcloudVirtualmachineShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Start(AzNetworkcloudVirtualmachineStartOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkcloudVirtualmachineStartOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Update(AzNetworkcloudVirtualmachineUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkcloudVirtualmachineUpdateOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Wait(AzNetworkcloudVirtualmachineWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkcloudVirtualmachineWaitOptions(), cancellationToken: token);
    }
}