using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "manager", "security-admin-config", "rule-collection", "show")]
public record AzNetworkManagerSecurityAdminConfigRuleCollectionShowOptions : AzOptions
{
    [CliOption("--configuration-name")]
    public string? ConfigurationName { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--rule-collection-name")]
    public string? RuleCollectionName { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}