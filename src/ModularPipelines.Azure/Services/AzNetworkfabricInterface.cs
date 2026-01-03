using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("networkfabric")]
public class AzNetworkfabricInterface
{
    public AzNetworkfabricInterface(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Delete(AzNetworkfabricInterfaceDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkfabricInterfaceDeleteOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> List(AzNetworkfabricInterfaceListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzNetworkfabricInterfaceShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkfabricInterfaceShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> UpdateAdminState(AzNetworkfabricInterfaceUpdateAdminStateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkfabricInterfaceUpdateAdminStateOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Wait(AzNetworkfabricInterfaceWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkfabricInterfaceWaitOptions(), cancellationToken: token);
    }
}