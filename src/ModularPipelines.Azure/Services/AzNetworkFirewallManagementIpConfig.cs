using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "firewall")]
public class AzNetworkFirewallManagementIpConfig
{
    public AzNetworkFirewallManagementIpConfig(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Show(AzNetworkFirewallManagementIpConfigShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkFirewallManagementIpConfigShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzNetworkFirewallManagementIpConfigUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkFirewallManagementIpConfigUpdateOptions(), token);
    }
}