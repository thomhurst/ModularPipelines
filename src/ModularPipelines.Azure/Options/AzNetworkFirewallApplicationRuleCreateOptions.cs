using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "firewall", "application-rule", "create")]
public record AzNetworkFirewallApplicationRuleCreateOptions(
[property: CliOption("--collection-name")] string CollectionName,
[property: CliOption("--firewall-name")] string FirewallName,
[property: CliOption("--name")] string Name,
[property: CliOption("--protocols")] string Protocols,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--action")]
    public string? Action { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--fqdn-tags")]
    public string? FqdnTags { get; set; }

    [CliOption("--priority")]
    public string? Priority { get; set; }

    [CliOption("--source-addresses")]
    public string? SourceAddresses { get; set; }

    [CliOption("--source-ip-groups")]
    public string? SourceIpGroups { get; set; }

    [CliOption("--target-fqdns")]
    public string? TargetFqdns { get; set; }
}