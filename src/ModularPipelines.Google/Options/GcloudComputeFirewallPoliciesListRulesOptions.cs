using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "firewall-policies", "list-rules")]
public record GcloudComputeFirewallPoliciesListRulesOptions(
[property: CliArgument] string FirewallPolicy,
[property: CliArgument] string Name
) : GcloudOptions
{
    [CliOption("--organization")]
    public string? Organization { get; set; }

    [CliOption("--regexp")]
    public string? Regexp { get; set; }
}