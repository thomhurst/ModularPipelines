using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("networkcloud", "virtualmachine")]
public class AzNetworkcloudVirtualmachineConsole
{
    public AzNetworkcloudVirtualmachineConsole(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzNetworkcloudVirtualmachineConsoleCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzNetworkcloudVirtualmachineConsoleDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkcloudVirtualmachineConsoleDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzNetworkcloudVirtualmachineConsoleListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzNetworkcloudVirtualmachineConsoleShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkcloudVirtualmachineConsoleShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzNetworkcloudVirtualmachineConsoleUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkcloudVirtualmachineConsoleUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzNetworkcloudVirtualmachineConsoleWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkcloudVirtualmachineConsoleWaitOptions(), token);
    }
}