using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "fleet", "fleetobservability", "disable")]
public record GcloudContainerFleetFleetobservabilityDisableOptions : GcloudOptions
{
    [CliFlag("--force")]
    public bool? Force { get; set; }
}