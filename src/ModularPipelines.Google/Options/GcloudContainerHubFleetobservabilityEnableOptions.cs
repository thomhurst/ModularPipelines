using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "hub", "fleetobservability", "enable")]
public record GcloudContainerHubFleetobservabilityEnableOptions : GcloudOptions;