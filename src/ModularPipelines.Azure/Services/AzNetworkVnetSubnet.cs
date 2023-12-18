using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "vnet")]
public class AzNetworkVnetSubnet
{
    public AzNetworkVnetSubnet(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzNetworkVnetSubnetCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzNetworkVnetSubnetDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkVnetSubnetDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzNetworkVnetSubnetListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListAvailableDelegations(AzNetworkVnetSubnetListAvailableDelegationsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkVnetSubnetListAvailableDelegationsOptions(), token);
    }

    public async Task<CommandResult> ListAvailableIps(AzNetworkVnetSubnetListAvailableIpsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkVnetSubnetListAvailableIpsOptions(), token);
    }

    public async Task<CommandResult> Show(AzNetworkVnetSubnetShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkVnetSubnetShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzNetworkVnetSubnetUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkVnetSubnetUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzNetworkVnetSubnetWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkVnetSubnetWaitOptions(), token);
    }
}