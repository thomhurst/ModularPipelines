using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network")]
public class AzNetworkVnet
{
    public AzNetworkVnet(
        AzNetworkVnetPeering peering,
        AzNetworkVnetSubnet subnet,
        AzNetworkVnetTap tap,
        ICommand internalCommand
    )
    {
        Peering = peering;
        Subnet = subnet;
        Tap = tap;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzNetworkVnetPeering Peering { get; }

    public AzNetworkVnetSubnet Subnet { get; }

    public AzNetworkVnetTap Tap { get; }

    public async Task<CommandResult> CheckIpAddress(AzNetworkVnetCheckIpAddressOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Create(AzNetworkVnetCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzNetworkVnetDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzNetworkVnetListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListAvailableIps(AzNetworkVnetListAvailableIpsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListEndpointServices(AzNetworkVnetListEndpointServicesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzNetworkVnetShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkVnetShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzNetworkVnetUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkVnetUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzNetworkVnetWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkVnetWaitOptions(), token);
    }
}