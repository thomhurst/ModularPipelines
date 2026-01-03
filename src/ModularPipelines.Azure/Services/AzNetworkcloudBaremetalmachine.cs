using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("networkcloud")]
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
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkcloudBaremetalmachineCordonOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> List(AzNetworkcloudBaremetalmachineListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkcloudBaremetalmachineListOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> PowerOff(AzNetworkcloudBaremetalmachinePowerOffOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkcloudBaremetalmachinePowerOffOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Reimage(AzNetworkcloudBaremetalmachineReimageOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkcloudBaremetalmachineReimageOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Replace(AzNetworkcloudBaremetalmachineReplaceOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkcloudBaremetalmachineReplaceOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Restart(AzNetworkcloudBaremetalmachineRestartOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkcloudBaremetalmachineRestartOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> RunCommand(AzNetworkcloudBaremetalmachineRunCommandOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> RunDataExtract(AzNetworkcloudBaremetalmachineRunDataExtractOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> RunReadCommand(AzNetworkcloudBaremetalmachineRunReadCommandOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzNetworkcloudBaremetalmachineShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkcloudBaremetalmachineShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Start(AzNetworkcloudBaremetalmachineStartOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkcloudBaremetalmachineStartOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Uncordon(AzNetworkcloudBaremetalmachineUncordonOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkcloudBaremetalmachineUncordonOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Update(AzNetworkcloudBaremetalmachineUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkcloudBaremetalmachineUpdateOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Wait(AzNetworkcloudBaremetalmachineWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkcloudBaremetalmachineWaitOptions(), cancellationToken: token);
    }
}