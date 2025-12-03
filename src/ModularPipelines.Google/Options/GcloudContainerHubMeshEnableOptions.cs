using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "hub", "mesh", "enable")]
public record GcloudContainerHubMeshEnableOptions : GcloudOptions
{
    [CliOption("--fleet-default-member-config")]
    public string? FleetDefaultMemberConfig { get; set; }
}