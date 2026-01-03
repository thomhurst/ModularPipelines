using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("networkfabric")]
public class AzNetworkfabricTaprule
{
    public AzNetworkfabricTaprule(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzNetworkfabricTapruleCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Delete(AzNetworkfabricTapruleDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkfabricTapruleDeleteOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> List(AzNetworkfabricTapruleListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkfabricTapruleListOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzNetworkfabricTapruleShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkfabricTapruleShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Update(AzNetworkfabricTapruleUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkfabricTapruleUpdateOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Wait(AzNetworkfabricTapruleWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkfabricTapruleWaitOptions(), cancellationToken: token);
    }
}