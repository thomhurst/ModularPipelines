using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network")]
public class AzNetworkCrossRegionLb
{
    public AzNetworkCrossRegionLb(
        AzNetworkCrossRegionLbAddressPool addressPool,
        AzNetworkCrossRegionLbFrontendIp frontendIp,
        AzNetworkCrossRegionLbRule rule,
        ICommand internalCommand
    )
    {
        AddressPool = addressPool;
        FrontendIp = frontendIp;
        Rule = rule;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzNetworkCrossRegionLbAddressPool AddressPool { get; }

    public AzNetworkCrossRegionLbFrontendIp FrontendIp { get; }

    public AzNetworkCrossRegionLbRule Rule { get; }

    public async Task<CommandResult> Create(AzNetworkCrossRegionLbCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzNetworkCrossRegionLbDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkCrossRegionLbDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzNetworkCrossRegionLbListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkCrossRegionLbListOptions(), token);
    }

    public async Task<CommandResult> Show(AzNetworkCrossRegionLbShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkCrossRegionLbShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzNetworkCrossRegionLbUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkCrossRegionLbUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzNetworkCrossRegionLbWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkCrossRegionLbWaitOptions(), token);
    }
}