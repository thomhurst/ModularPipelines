using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "manager", "security-admin-config", "rule-collection", "rule", "update")]
public record AzNetworkManagerSecurityAdminConfigRuleCollectionRuleUpdateOptions(
[property: CommandSwitch("--rule-collection-name")] string RuleCollectionName
) : AzOptions
{
    [CommandSwitch("--access")]
    public string? Access { get; set; }

    [CommandSwitch("--configuration-name")]
    public string? ConfigurationName { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--dest-port-ranges")]
    public string? DestPortRanges { get; set; }

    [CommandSwitch("--destinations")]
    public string? Destinations { get; set; }

    [CommandSwitch("--direction")]
    public string? Direction { get; set; }

    [CommandSwitch("--flag")]
    public string? Flag { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--kind")]
    public string? Kind { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--priority")]
    public string? Priority { get; set; }

    [CommandSwitch("--protocol")]
    public string? Protocol { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--rule-name")]
    public string? RuleName { get; set; }

    [CommandSwitch("--source-port-ranges")]
    public string? SourcePortRanges { get; set; }

    [CommandSwitch("--sources")]
    public string? Sources { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}