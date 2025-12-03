using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "network-firewall-policies", "associations", "create")]
public record GcloudComputeNetworkFirewallPoliciesAssociationsCreateOptions(
[property: CliOption("--firewall-policy")] string FirewallPolicy,
[property: CliOption("--network")] string Network
) : GcloudOptions
{
    [CliOption("--name")]
    public string? Name { get; set; }

    [CliFlag("--replace-association-on-target")]
    public bool? ReplaceAssociationOnTarget { get; set; }

    [CliOption("--firewall-policy-region")]
    public string? FirewallPolicyRegion { get; set; }

    [CliFlag("--global-firewall-policy")]
    public bool? GlobalFirewallPolicy { get; set; }
}