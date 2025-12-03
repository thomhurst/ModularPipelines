using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "firewall-policies", "rules", "describe")]
public record GcloudComputeFirewallPoliciesRulesDescribeOptions(
[property: CliArgument] string Priority,
[property: CliOption("--firewall-policy")] string FirewallPolicy
) : GcloudOptions
{
    [CliOption("--organization")]
    public string? Organization { get; set; }
}