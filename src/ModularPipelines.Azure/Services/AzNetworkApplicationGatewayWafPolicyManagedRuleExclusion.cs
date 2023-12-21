using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "application-gateway", "waf-policy", "managed-rule")]
public class AzNetworkApplicationGatewayWafPolicyManagedRuleExclusion
{
    public AzNetworkApplicationGatewayWafPolicyManagedRuleExclusion(
        AzNetworkApplicationGatewayWafPolicyManagedRuleExclusionRuleSet ruleSet,
        ICommand internalCommand
    )
    {
        RuleSet = ruleSet;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzNetworkApplicationGatewayWafPolicyManagedRuleExclusionRuleSet RuleSet { get; }

    public async Task<CommandResult> Add(AzNetworkApplicationGatewayWafPolicyManagedRuleExclusionAddOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzNetworkApplicationGatewayWafPolicyManagedRuleExclusionListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Remove(AzNetworkApplicationGatewayWafPolicyManagedRuleExclusionRemoveOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}