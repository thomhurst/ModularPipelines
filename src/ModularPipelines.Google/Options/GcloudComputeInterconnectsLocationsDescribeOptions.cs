using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "interconnects", "locations", "describe")]
public record GcloudComputeInterconnectsLocationsDescribeOptions(
[property: PositionalArgument] string Name
) : GcloudOptions;