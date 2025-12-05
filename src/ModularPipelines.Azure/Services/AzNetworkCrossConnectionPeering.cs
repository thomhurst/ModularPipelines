using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("network", "cross-connection")]
public class AzNetworkCrossConnectionPeering
{
    public AzNetworkCrossConnectionPeering(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzNetworkCrossConnectionPeeringCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzNetworkCrossConnectionPeeringDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkCrossConnectionPeeringDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzNetworkCrossConnectionPeeringListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzNetworkCrossConnectionPeeringShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkCrossConnectionPeeringShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzNetworkCrossConnectionPeeringUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkCrossConnectionPeeringUpdateOptions(), token);
    }
}