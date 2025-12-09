using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "manager", "security-admin-config", "rule-collection", "create")]
public record AzNetworkManagerSecurityAdminConfigRuleCollectionCreateOptions(
[property: CliOption("--applies-to-groups")] string AppliesToGroups,
[property: CliOption("--configuration-name")] string ConfigurationName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--rule-collection-name")] string RuleCollectionName
) : AzOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }
}