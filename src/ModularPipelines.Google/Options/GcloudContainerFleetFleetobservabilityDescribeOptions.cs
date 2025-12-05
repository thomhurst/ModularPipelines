using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "fleet", "fleetobservability", "describe")]
public record GcloudContainerFleetFleetobservabilityDescribeOptions : GcloudOptions;