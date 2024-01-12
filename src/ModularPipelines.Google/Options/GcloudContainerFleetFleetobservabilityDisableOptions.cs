using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "fleet", "fleetobservability", "disable")]
public record GcloudContainerFleetFleetobservabilityDisableOptions : GcloudOptions
{
    [BooleanCommandSwitch("--force")]
    public bool? Force { get; set; }
}