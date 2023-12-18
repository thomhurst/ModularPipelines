using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "application-gateway", "waf-policy", "managed-rule")]
public class AzNetworkApplicationGatewayWafPolicyManagedRuleRuleSet
{
    public AzNetworkApplicationGatewayWafPolicyManagedRuleRuleSet(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Add(AzNetworkApplicationGatewayWafPolicyManagedRuleRuleSetAddOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzNetworkApplicationGatewayWafPolicyManagedRuleRuleSetListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Remove(AzNetworkApplicationGatewayWafPolicyManagedRuleRuleSetRemoveOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(AzNetworkApplicationGatewayWafPolicyManagedRuleRuleSetUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}