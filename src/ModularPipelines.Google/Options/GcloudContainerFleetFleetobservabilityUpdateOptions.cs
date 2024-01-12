using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "fleet", "fleetobservability", "update")]
public record GcloudContainerFleetFleetobservabilityUpdateOptions : GcloudOptions
{
    [CommandSwitch("--logging-config")]
    public string? LoggingConfig { get; set; }
}