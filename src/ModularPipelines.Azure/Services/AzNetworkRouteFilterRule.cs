using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("network", "route-filter")]
public class AzNetworkRouteFilterRule
{
    public AzNetworkRouteFilterRule(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzNetworkRouteFilterRuleCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Delete(AzNetworkRouteFilterRuleDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkRouteFilterRuleDeleteOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> List(AzNetworkRouteFilterRuleListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> ListServiceCommunities(AzNetworkRouteFilterRuleListServiceCommunitiesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkRouteFilterRuleListServiceCommunitiesOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzNetworkRouteFilterRuleShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkRouteFilterRuleShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Update(AzNetworkRouteFilterRuleUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkRouteFilterRuleUpdateOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Wait(AzNetworkRouteFilterRuleWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkRouteFilterRuleWaitOptions(), cancellationToken: token);
    }
}