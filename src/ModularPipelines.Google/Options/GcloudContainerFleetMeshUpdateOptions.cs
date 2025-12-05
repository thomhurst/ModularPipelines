using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "fleet", "mesh", "update")]
public record GcloudContainerFleetMeshUpdateOptions(
[property: CliOption("--fleet-default-member-config")] string FleetDefaultMemberConfig,
[property: CliOption("--control-plane")] string ControlPlane,
[property: CliOption("--management")] string Management,
[property: CliOption("--origin")] string Origin,
[property: CliOption("--memberships")] string[] Memberships,
[property: CliOption("--location")] string Location
) : GcloudOptions;