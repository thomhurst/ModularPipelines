using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "hub", "mesh", "disable")]
public record GcloudContainerHubMeshDisableOptions : GcloudOptions
{
    [BooleanCommandSwitch("--fleet-default-member-config")]
    public bool? FleetDefaultMemberConfig { get; set; }

    [BooleanCommandSwitch("--force")]
    public bool? Force { get; set; }
}