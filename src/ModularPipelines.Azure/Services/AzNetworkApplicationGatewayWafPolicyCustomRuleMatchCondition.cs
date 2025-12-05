using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("network", "application-gateway", "waf-policy", "custom-rule")]
public class AzNetworkApplicationGatewayWafPolicyCustomRuleMatchCondition
{
    public AzNetworkApplicationGatewayWafPolicyCustomRuleMatchCondition(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Add(AzNetworkApplicationGatewayWafPolicyCustomRuleMatchConditionAddOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzNetworkApplicationGatewayWafPolicyCustomRuleMatchConditionListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Remove(AzNetworkApplicationGatewayWafPolicyCustomRuleMatchConditionRemoveOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}