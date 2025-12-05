using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "firewall-policies", "create")]
public record GcloudComputeFirewallPoliciesCreateOptions(
[property: CliOption("--short-name")] string ShortName,
[property: CliOption("--folder")] string Folder,
[property: CliOption("--organization")] string Organization
) : GcloudOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }
}