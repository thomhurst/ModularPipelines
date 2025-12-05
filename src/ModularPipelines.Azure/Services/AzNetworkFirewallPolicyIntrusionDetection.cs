using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("network", "firewall", "policy")]
public class AzNetworkFirewallPolicyIntrusionDetection
{
    public AzNetworkFirewallPolicyIntrusionDetection(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Add(AzNetworkFirewallPolicyIntrusionDetectionAddOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkFirewallPolicyIntrusionDetectionAddOptions(), token);
    }

    public async Task<CommandResult> List(AzNetworkFirewallPolicyIntrusionDetectionListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Remove(AzNetworkFirewallPolicyIntrusionDetectionRemoveOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkFirewallPolicyIntrusionDetectionRemoveOptions(), token);
    }
}