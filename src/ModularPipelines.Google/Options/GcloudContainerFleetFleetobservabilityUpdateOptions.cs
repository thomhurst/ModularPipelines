using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "fleet", "fleetobservability", "update")]
public record GcloudContainerFleetFleetobservabilityUpdateOptions : GcloudOptions
{
    [CliOption("--logging-config")]
    public string? LoggingConfig { get; set; }
}