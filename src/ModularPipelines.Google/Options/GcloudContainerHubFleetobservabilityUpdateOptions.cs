using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "hub", "fleetobservability", "update")]
public record GcloudContainerHubFleetobservabilityUpdateOptions : GcloudOptions
{
    [CliOption("--logging-config")]
    public string? LoggingConfig { get; set; }
}