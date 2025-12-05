using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "firewall-policies", "list")]
public record GcloudComputeFirewallPoliciesListOptions(
[property: CliArgument] string Name,
[property: CliOption("--folder")] string Folder,
[property: CliOption("--organization")] string Organization
) : GcloudOptions
{
    [CliOption("--regexp")]
    public string? Regexp { get; set; }
}