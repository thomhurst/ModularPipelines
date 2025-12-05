using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "fleet", "mesh", "enable")]
public record GcloudContainerFleetMeshEnableOptions : GcloudOptions
{
    [CliOption("--fleet-default-member-config")]
    public string? FleetDefaultMemberConfig { get; set; }
}