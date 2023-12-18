using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network")]
public class AzNetworkRouteFilter
{
    public AzNetworkRouteFilter(
        AzNetworkRouteFilterRule rule,
        ICommand internalCommand
    )
    {
        Rule = rule;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzNetworkRouteFilterRule Rule { get; }

    public async Task<CommandResult> Create(AzNetworkRouteFilterCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzNetworkRouteFilterDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkRouteFilterDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzNetworkRouteFilterListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkRouteFilterListOptions(), token);
    }

    public async Task<CommandResult> Show(AzNetworkRouteFilterShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkRouteFilterShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzNetworkRouteFilterUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkRouteFilterUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzNetworkRouteFilterWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkRouteFilterWaitOptions(), token);
    }
}