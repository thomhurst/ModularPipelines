using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "interconnects", "remote-locations", "describe")]
public record GcloudComputeInterconnectsRemoteLocationsDescribeOptions(
[property: CliArgument] string Name
) : GcloudOptions;