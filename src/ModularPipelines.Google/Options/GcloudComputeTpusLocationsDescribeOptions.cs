using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "tpus", "locations", "describe")]
public record GcloudComputeTpusLocationsDescribeOptions(
[property: PositionalArgument] string Zone
) : GcloudOptions;