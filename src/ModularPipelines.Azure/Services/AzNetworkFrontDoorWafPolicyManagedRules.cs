using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "front-door", "waf-policy")]
public class AzNetworkFrontDoorWafPolicyManagedRules
{
    public AzNetworkFrontDoorWafPolicyManagedRules(
        AzNetworkFrontDoorWafPolicyManagedRulesExclusion exclusion,
        AzNetworkFrontDoorWafPolicyManagedRulesOverride @override,
        ICommand internalCommand
    )
    {
        Exclusion = exclusion;
        Override = @override;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzNetworkFrontDoorWafPolicyManagedRulesExclusion Exclusion { get; }

    public AzNetworkFrontDoorWafPolicyManagedRulesOverride Override { get; }

    public async Task<CommandResult> Add(AzNetworkFrontDoorWafPolicyManagedRulesAddOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzNetworkFrontDoorWafPolicyManagedRulesListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Remove(AzNetworkFrontDoorWafPolicyManagedRulesRemoveOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}