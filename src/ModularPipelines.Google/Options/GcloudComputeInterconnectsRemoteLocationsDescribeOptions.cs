using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "interconnects", "remote-locations", "describe")]
public record GcloudComputeInterconnectsRemoteLocationsDescribeOptions(
[property: PositionalArgument] string Name
) : GcloudOptions;