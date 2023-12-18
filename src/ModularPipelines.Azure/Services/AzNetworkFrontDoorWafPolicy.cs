using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "front-door")]
public class AzNetworkFrontDoorWafPolicy
{
    public AzNetworkFrontDoorWafPolicy(
        AzNetworkFrontDoorWafPolicyManagedRuleDefinition managedRuleDefinition,
        AzNetworkFrontDoorWafPolicyManagedRules managedRules,
        AzNetworkFrontDoorWafPolicyRule rule,
        ICommand internalCommand
    )
    {
        ManagedRuleDefinition = managedRuleDefinition;
        ManagedRules = managedRules;
        Rule = rule;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzNetworkFrontDoorWafPolicyManagedRuleDefinition ManagedRuleDefinition { get; }

    public AzNetworkFrontDoorWafPolicyManagedRules ManagedRules { get; }

    public AzNetworkFrontDoorWafPolicyRule Rule { get; }

    public async Task<CommandResult> Create(AzNetworkFrontDoorWafPolicyCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzNetworkFrontDoorWafPolicyDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzNetworkFrontDoorWafPolicyListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzNetworkFrontDoorWafPolicyShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkFrontDoorWafPolicyShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzNetworkFrontDoorWafPolicyUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkFrontDoorWafPolicyUpdateOptions(), token);
    }
}