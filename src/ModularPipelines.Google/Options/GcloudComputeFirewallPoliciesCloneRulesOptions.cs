using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "firewall-policies", "clone-rules")]
public record GcloudComputeFirewallPoliciesCloneRulesOptions(
[property: CliArgument] string FirewallPolicy,
[property: CliOption("--source-firewall-policy")] string SourceFirewallPolicy
) : GcloudOptions
{
    [CliOption("--organization")]
    public string? Organization { get; set; }
}