using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "vhub")]
public class AzNetworkVhubRouteMap
{
    public AzNetworkVhubRouteMap(
        AzNetworkVhubRouteMapRule rule,
        ICommand internalCommand
    )
    {
        Rule = rule;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzNetworkVhubRouteMapRule Rule { get; }

    public async Task<CommandResult> Create(AzNetworkVhubRouteMapCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzNetworkVhubRouteMapDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkVhubRouteMapDeleteOptions(), token);
    }

    public async Task<CommandResult> GetInboundRoutes(AzNetworkVhubRouteMapGetInboundRoutesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkVhubRouteMapGetInboundRoutesOptions(), token);
    }

    public async Task<CommandResult> GetOutboundRoutes(AzNetworkVhubRouteMapGetOutboundRoutesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkVhubRouteMapGetOutboundRoutesOptions(), token);
    }

    public async Task<CommandResult> List(AzNetworkVhubRouteMapListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzNetworkVhubRouteMapShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkVhubRouteMapShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzNetworkVhubRouteMapUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkVhubRouteMapUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzNetworkVhubRouteMapWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkVhubRouteMapWaitOptions(), token);
    }
}