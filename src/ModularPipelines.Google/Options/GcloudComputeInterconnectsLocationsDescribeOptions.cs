using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "interconnects", "locations", "describe")]
public record GcloudComputeInterconnectsLocationsDescribeOptions(
[property: CliArgument] string Name
) : GcloudOptions;