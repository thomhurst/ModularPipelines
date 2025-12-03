using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "manager", "security-admin-config", "rule-collection", "rule", "update")]
public record AzNetworkManagerSecurityAdminConfigRuleCollectionRuleUpdateOptions(
[property: CliOption("--rule-collection-name")] string RuleCollectionName
) : AzOptions
{
    [CliOption("--access")]
    public string? Access { get; set; }

    [CliOption("--configuration-name")]
    public string? ConfigurationName { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--dest-port-ranges")]
    public string? DestPortRanges { get; set; }

    [CliOption("--destinations")]
    public string? Destinations { get; set; }

    [CliOption("--direction")]
    public string? Direction { get; set; }

    [CliOption("--flag")]
    public string? Flag { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--kind")]
    public string? Kind { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--priority")]
    public string? Priority { get; set; }

    [CliOption("--protocol")]
    public string? Protocol { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--rule-name")]
    public string? RuleName { get; set; }

    [CliOption("--source-port-ranges")]
    public string? SourcePortRanges { get; set; }

    [CliOption("--sources")]
    public string? Sources { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}