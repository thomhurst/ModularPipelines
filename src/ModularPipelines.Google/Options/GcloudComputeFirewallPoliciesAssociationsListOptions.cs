using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "firewall-policies", "associations", "list")]
public record GcloudComputeFirewallPoliciesAssociationsListOptions(
[property: CommandSwitch("--folder")] string Folder,
[property: CommandSwitch("--organization")] string Organization
) : GcloudOptions;