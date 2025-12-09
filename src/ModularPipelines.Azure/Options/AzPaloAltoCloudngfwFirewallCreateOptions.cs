using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("palo-alto", "cloudngfw", "firewall", "create")]
public record AzPaloAltoCloudngfwFirewallCreateOptions(
[property: CliOption("--dns-settings")] string DnsSettings,
[property: CliOption("--firewall-name")] string FirewallName,
[property: CliOption("--marketplace-details")] string MarketplaceDetails,
[property: CliOption("--network-profile")] string NetworkProfile,
[property: CliOption("--plan-data")] string PlanData,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--associated-rulestack")]
    public string? AssociatedRulestack { get; set; }

    [CliOption("--front-end-settings")]
    public string? FrontEndSettings { get; set; }

    [CliOption("--identity")]
    public string? Identity { get; set; }

    [CliFlag("--is-panorama-managed")]
    public bool? IsPanoramaManaged { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--pan-etag")]
    public string? PanEtag { get; set; }

    [CliOption("--panorama-config")]
    public string? PanoramaConfig { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}