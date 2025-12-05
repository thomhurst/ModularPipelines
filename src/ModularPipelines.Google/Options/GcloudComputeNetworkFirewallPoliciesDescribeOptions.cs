using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "network-firewall-policies", "describe")]
public record GcloudComputeNetworkFirewallPoliciesDescribeOptions(
[property: CliArgument] string FirewallPolicy
) : GcloudOptions
{
    [CliFlag("--global")]
    public bool? Global { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }
}