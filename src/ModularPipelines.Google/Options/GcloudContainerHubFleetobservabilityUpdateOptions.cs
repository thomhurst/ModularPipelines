using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "hub", "fleetobservability", "update")]
public record GcloudContainerHubFleetobservabilityUpdateOptions : GcloudOptions
{
    [CommandSwitch("--logging-config")]
    public string? LoggingConfig { get; set; }
}