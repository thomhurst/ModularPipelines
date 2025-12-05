using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "network-firewall-policies", "clone-rules")]
public record GcloudComputeNetworkFirewallPoliciesCloneRulesOptions(
[property: CliArgument] string FirewallPolicy,
[property: CliOption("--source-firewall-policy")] string SourceFirewallPolicy
) : GcloudOptions
{
    [CliFlag("--global")]
    public bool? Global { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }
}