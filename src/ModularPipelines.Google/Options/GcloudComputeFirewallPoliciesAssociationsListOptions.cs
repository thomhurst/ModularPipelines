using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "firewall-policies", "associations", "list")]
public record GcloudComputeFirewallPoliciesAssociationsListOptions(
[property: CliOption("--folder")] string Folder,
[property: CliOption("--organization")] string Organization
) : GcloudOptions;