using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "hub", "mesh", "disable")]
public record GcloudContainerHubMeshDisableOptions : GcloudOptions
{
    [CliFlag("--fleet-default-member-config")]
    public bool? FleetDefaultMemberConfig { get; set; }

    [CliFlag("--force")]
    public bool? Force { get; set; }
}