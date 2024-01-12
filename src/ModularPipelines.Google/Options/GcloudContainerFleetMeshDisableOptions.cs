using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "fleet", "mesh", "disable")]
public record GcloudContainerFleetMeshDisableOptions : GcloudOptions
{
    [BooleanCommandSwitch("--fleet-default-member-config")]
    public bool? FleetDefaultMemberConfig { get; set; }

    [BooleanCommandSwitch("--force")]
    public bool? Force { get; set; }
}