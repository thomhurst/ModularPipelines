using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("networkcloud")]
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
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzNetworkcloudVirtualmachineDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkcloudVirtualmachineDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzNetworkcloudVirtualmachineListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkcloudVirtualmachineListOptions(), token);
    }

    public async Task<CommandResult> PowerOff(AzNetworkcloudVirtualmachinePowerOffOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkcloudVirtualmachinePowerOffOptions(), token);
    }

    public async Task<CommandResult> Reimage(AzNetworkcloudVirtualmachineReimageOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkcloudVirtualmachineReimageOptions(), token);
    }

    public async Task<CommandResult> Restart(AzNetworkcloudVirtualmachineRestartOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkcloudVirtualmachineRestartOptions(), token);
    }

    public async Task<CommandResult> Show(AzNetworkcloudVirtualmachineShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkcloudVirtualmachineShowOptions(), token);
    }

    public async Task<CommandResult> Start(AzNetworkcloudVirtualmachineStartOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkcloudVirtualmachineStartOptions(), token);
    }

    public async Task<CommandResult> Update(AzNetworkcloudVirtualmachineUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkcloudVirtualmachineUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzNetworkcloudVirtualmachineWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkcloudVirtualmachineWaitOptions(), token);
    }
}