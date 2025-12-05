using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "network-firewall-policies", "get-effective-firewalls")]
public record GcloudComputeNetworkFirewallPoliciesGetEffectiveFirewallsOptions(
[property: CliOption("--network")] string Network
) : GcloudOptions
{
    [CliOption("--region")]
    public string? Region { get; set; }
}