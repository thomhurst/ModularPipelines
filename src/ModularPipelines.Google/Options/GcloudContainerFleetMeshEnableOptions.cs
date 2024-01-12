using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "fleet", "mesh", "enable")]
public record GcloudContainerFleetMeshEnableOptions : GcloudOptions
{
    [CommandSwitch("--fleet-default-member-config")]
    public string? FleetDefaultMemberConfig { get; set; }
}