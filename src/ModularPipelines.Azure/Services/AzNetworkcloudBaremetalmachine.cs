using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

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

    public async Task<CommandResult> Cordon(AzNetworkcloudBaremetalmachineCordonOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzNetworkcloudBaremetalmachineListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PowerOff(AzNetworkcloudBaremetalmachinePowerOffOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Reimage(AzNetworkcloudBaremetalmachineReimageOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Replace(AzNetworkcloudBaremetalmachineReplaceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Restart(AzNetworkcloudBaremetalmachineRestartOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
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