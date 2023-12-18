using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "manager", "security-admin-config", "rule-collection", "create")]
public record AzNetworkManagerSecurityAdminConfigRuleCollectionCreateOptions(
[property: CommandSwitch("--applies-to-groups")] string AppliesToGroups,
[property: CommandSwitch("--configuration-name")] string ConfigurationName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--rule-collection-name")] string RuleCollectionName
) : AzOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }
}