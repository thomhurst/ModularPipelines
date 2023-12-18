using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("networkcloud")]
public class AzNetworkcloudBaremetalmachine
{
    public AzNetworkcloudBaremetalmachine(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Cordon(AzNetworkcloudBaremetalmachineCordonOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkcloudBaremetalmachineCordonOptions(), token);
    }

    public async Task<CommandResult> List(AzNetworkcloudBaremetalmachineListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkcloudBaremetalmachineListOptions(), token);
    }

    public async Task<CommandResult> PowerOff(AzNetworkcloudBaremetalmachinePowerOffOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkcloudBaremetalmachinePowerOffOptions(), token);
    }

    public async Task<CommandResult> Reimage(AzNetworkcloudBaremetalmachineReimageOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkcloudBaremetalmachineReimageOptions(), token);
    }

    public async Task<CommandResult> Replace(AzNetworkcloudBaremetalmachineReplaceOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkcloudBaremetalmachineReplaceOptions(), token);
    }

    public async Task<CommandResult> Restart(AzNetworkcloudBaremetalmachineRestartOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkcloudBaremetalmachineRestartOptions(), token);
    }

    public async Task<CommandResult> RunCommand(AzNetworkcloudBaremetalmachineRunCommandOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RunDataExtract(AzNetworkcloudBaremetalmachineRunDataExtractOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RunReadCommand(AzNetworkcloudBaremetalmachineRunReadCommandOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzNetworkcloudBaremetalmachineShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkcloudBaremetalmachineShowOptions(), token);
    }

    public async Task<CommandResult> Start(AzNetworkcloudBaremetalmachineStartOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkcloudBaremetalmachineStartOptions(), token);
    }

    public async Task<CommandResult> Uncordon(AzNetworkcloudBaremetalmachineUncordonOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkcloudBaremetalmachineUncordonOptions(), token);
    }

    public async Task<CommandResult> Update(AzNetworkcloudBaremetalmachineUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkcloudBaremetalmachineUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzNetworkcloudBaremetalmachineWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkcloudBaremetalmachineWaitOptions(), token);
    }
}