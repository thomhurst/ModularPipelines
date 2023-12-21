using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connectedvmware")]
public class AzConnectedvmwareVirtualNetwork
{
    public AzConnectedvmwareVirtualNetwork(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzConnectedvmwareVirtualNetworkCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzConnectedvmwareVirtualNetworkDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConnectedvmwareVirtualNetworkDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzConnectedvmwareVirtualNetworkListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConnectedvmwareVirtualNetworkListOptions(), token);
    }

    public async Task<CommandResult> Show(AzConnectedvmwareVirtualNetworkShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConnectedvmwareVirtualNetworkShowOptions(), token);
    }
}