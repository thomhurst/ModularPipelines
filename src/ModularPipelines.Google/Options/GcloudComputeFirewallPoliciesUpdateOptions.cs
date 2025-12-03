using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "firewall-policies", "update")]
public record GcloudComputeFirewallPoliciesUpdateOptions(
[property: CliArgument] string FirewallPolicy
) : GcloudOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--organization")]
    public string? Organization { get; set; }
}