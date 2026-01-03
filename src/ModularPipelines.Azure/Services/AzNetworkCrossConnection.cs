using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("network")]
public class AzNetworkCrossConnection
{
    public AzNetworkCrossConnection(
        AzNetworkCrossConnectionPeering peering,
        ICommand internalCommand
    )
    {
        Peering = peering;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzNetworkCrossConnectionPeering Peering { get; }

    public async Task<CommandResult> List(AzNetworkCrossConnectionListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkCrossConnectionListOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> ListArpTables(AzNetworkCrossConnectionListArpTablesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> ListRouteTables(AzNetworkCrossConnectionListRouteTablesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzNetworkCrossConnectionShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkCrossConnectionShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> SummarizeRouteTable(AzNetworkCrossConnectionSummarizeRouteTableOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Update(AzNetworkCrossConnectionUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkCrossConnectionUpdateOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Wait(AzNetworkCrossConnectionWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkCrossConnectionWaitOptions(), cancellationToken: token);
    }
}