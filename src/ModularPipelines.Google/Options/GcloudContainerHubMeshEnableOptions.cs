using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "hub", "mesh", "enable")]
public record GcloudContainerHubMeshEnableOptions : GcloudOptions
{
    [CommandSwitch("--fleet-default-member-config")]
    public string? FleetDefaultMemberConfig { get; set; }
}