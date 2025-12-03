using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "fleet", "mesh", "disable")]
public record GcloudContainerFleetMeshDisableOptions : GcloudOptions
{
    [CliFlag("--fleet-default-member-config")]
    public bool? FleetDefaultMemberConfig { get; set; }

    [CliFlag("--force")]
    public bool? Force { get; set; }
}