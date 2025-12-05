using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "firewall-policies", "rules", "delete")]
public record GcloudComputeFirewallPoliciesRulesDeleteOptions(
[property: CliArgument] string Priority,
[property: CliOption("--firewall-policy")] string FirewallPolicy
) : GcloudOptions
{
    [CliOption("--organization")]
    public string? Organization { get; set; }
}