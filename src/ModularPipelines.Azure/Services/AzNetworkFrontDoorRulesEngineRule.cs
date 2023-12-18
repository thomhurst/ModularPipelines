using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "front-door", "rules-engine")]
public class AzNetworkFrontDoorRulesEngineRule
{
    public AzNetworkFrontDoorRulesEngineRule(
        AzNetworkFrontDoorRulesEngineRuleAction action,
        AzNetworkFrontDoorRulesEngineRuleCondition condition,
        ICommand internalCommand
    )
    {
        Action = action;
        Condition = condition;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzNetworkFrontDoorRulesEngineRuleAction Action { get; }

    public AzNetworkFrontDoorRulesEngineRuleCondition Condition { get; }

    public async Task<CommandResult> Create(AzNetworkFrontDoorRulesEngineRuleCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzNetworkFrontDoorRulesEngineRuleDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzNetworkFrontDoorRulesEngineRuleListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzNetworkFrontDoorRulesEngineRuleShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(AzNetworkFrontDoorRulesEngineRuleUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}