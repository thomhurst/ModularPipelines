using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "application-gateway", "waf-policy")]
public class AzNetworkApplicationGatewayWafPolicyCustomRule
{
    public AzNetworkApplicationGatewayWafPolicyCustomRule(
        AzNetworkApplicationGatewayWafPolicyCustomRuleMatchCondition matchCondition,
        ICommand internalCommand
    )
    {
        MatchCondition = matchCondition;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzNetworkApplicationGatewayWafPolicyCustomRuleMatchCondition MatchCondition { get; }

    public async Task<CommandResult> Create(AzNetworkApplicationGatewayWafPolicyCustomRuleCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzNetworkApplicationGatewayWafPolicyCustomRuleDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzNetworkApplicationGatewayWafPolicyCustomRuleListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzNetworkApplicationGatewayWafPolicyCustomRuleShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(AzNetworkApplicationGatewayWafPolicyCustomRuleUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}