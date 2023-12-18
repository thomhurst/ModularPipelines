using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "front-door", "waf-policy")]
public class AzNetworkFrontDoorWafPolicyManagedRuleDefinition
{
    public AzNetworkFrontDoorWafPolicyManagedRuleDefinition(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> List(AzNetworkFrontDoorWafPolicyManagedRuleDefinitionListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkFrontDoorWafPolicyManagedRuleDefinitionListOptions(), token);
    }
}