using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "manager", "security-admin-config", "rule-collection", "show")]
public record AzNetworkManagerSecurityAdminConfigRuleCollectionShowOptions : AzOptions
{
    [CommandSwitch("--configuration-name")]
    public string? ConfigurationName { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--rule-collection-name")]
    public string? RuleCollectionName { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}