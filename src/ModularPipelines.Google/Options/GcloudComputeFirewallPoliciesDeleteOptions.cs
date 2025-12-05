using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "firewall-policies", "delete")]
public record GcloudComputeFirewallPoliciesDeleteOptions(
[property: CliArgument] string FirewallPolicy
) : GcloudOptions
{
    [CliOption("--organization")]
    public string? Organization { get; set; }
}