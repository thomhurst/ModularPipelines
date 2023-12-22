using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("palo-alto", "cloudngfw", "firewall", "create")]
public record AzPaloAltoCloudngfwFirewallCreateOptions(
[property: CommandSwitch("--dns-settings")] string DnsSettings,
[property: CommandSwitch("--firewall-name")] string FirewallName,
[property: CommandSwitch("--marketplace-details")] string MarketplaceDetails,
[property: CommandSwitch("--network-profile")] string NetworkProfile,
[property: CommandSwitch("--plan-data")] string PlanData,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--associated-rulestack")]
    public string? AssociatedRulestack { get; set; }

    [CommandSwitch("--front-end-settings")]
    public string? FrontEndSettings { get; set; }

    [CommandSwitch("--identity")]
    public string? Identity { get; set; }

    [BooleanCommandSwitch("--is-panorama-managed")]
    public bool? IsPanoramaManaged { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--pan-etag")]
    public string? PanEtag { get; set; }

    [CommandSwitch("--panorama-config")]
    public string? PanoramaConfig { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}