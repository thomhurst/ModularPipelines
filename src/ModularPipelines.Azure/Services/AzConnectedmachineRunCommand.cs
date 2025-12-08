using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("connectedmachine")]
public class AzConnectedmachineRunCommand
{
    public AzConnectedmachineRunCommand(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzConnectedmachineRunCommandCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzConnectedmachineRunCommandDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConnectedmachineRunCommandDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzConnectedmachineRunCommandListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzConnectedmachineRunCommandShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConnectedmachineRunCommandShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzConnectedmachineRunCommandUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConnectedmachineRunCommandUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzConnectedmachineRunCommandWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConnectedmachineRunCommandWaitOptions(), token);
    }
}