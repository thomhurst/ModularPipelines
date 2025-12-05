using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "firewall-policies", "associations", "create")]
public record GcloudComputeFirewallPoliciesAssociationsCreateOptions(
[property: CliOption("--firewall-policy")] string FirewallPolicy
) : GcloudOptions
{
    [CliOption("--folder")]
    public string? Folder { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--organization")]
    public string? Organization { get; set; }

    [CliFlag("--replace-association-on-target")]
    public bool? ReplaceAssociationOnTarget { get; set; }
}