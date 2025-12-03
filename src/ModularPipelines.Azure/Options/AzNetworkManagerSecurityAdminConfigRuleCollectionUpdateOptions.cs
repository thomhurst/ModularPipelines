using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "manager", "security-admin-config", "rule-collection", "update")]
public record AzNetworkManagerSecurityAdminConfigRuleCollectionUpdateOptions(
[property: CliOption("--rule-collection-name")] string RuleCollectionName
) : AzOptions
{
    [CliOption("--applies-to-groups")]
    public string? AppliesToGroups { get; set; }

    [CliOption("--configuration-name")]
    public string? ConfigurationName { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}