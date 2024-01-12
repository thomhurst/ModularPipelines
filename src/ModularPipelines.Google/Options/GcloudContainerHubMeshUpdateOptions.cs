using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "hub", "mesh", "update")]
public record GcloudContainerHubMeshUpdateOptions(
[property: CommandSwitch("--fleet-default-member-config")] string FleetDefaultMemberConfig,
[property: CommandSwitch("--control-plane")] string ControlPlane,
[property: CommandSwitch("--management")] string Management,
[property: CommandSwitch("--origin")] string Origin,
[property: CommandSwitch("--memberships")] string[] Memberships,
[property: CommandSwitch("--location")] string Location
) : GcloudOptions;