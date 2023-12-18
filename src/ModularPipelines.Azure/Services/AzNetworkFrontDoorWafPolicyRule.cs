using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "front-door", "waf-policy")]
public class AzNetworkFrontDoorWafPolicyRule
{
    public AzNetworkFrontDoorWafPolicyRule(
        AzNetworkFrontDoorWafPolicyRuleMatchCondition matchCondition,
        ICommand internalCommand
    )
    {
        MatchCondition = matchCondition;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzNetworkFrontDoorWafPolicyRuleMatchCondition MatchCondition { get; }

    public async Task<CommandResult> Create(AzNetworkFrontDoorWafPolicyRuleCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzNetworkFrontDoorWafPolicyRuleDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzNetworkFrontDoorWafPolicyRuleListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzNetworkFrontDoorWafPolicyRuleShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkFrontDoorWafPolicyRuleShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzNetworkFrontDoorWafPolicyRuleUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkFrontDoorWafPolicyRuleUpdateOptions(), token);
    }
}