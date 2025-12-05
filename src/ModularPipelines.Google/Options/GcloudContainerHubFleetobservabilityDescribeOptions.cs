using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "hub", "fleetobservability", "describe")]
public record GcloudContainerHubFleetobservabilityDescribeOptions : GcloudOptions;