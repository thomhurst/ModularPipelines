using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "tpus", "locations", "describe")]
public record GcloudComputeTpusLocationsDescribeOptions(
[property: CliArgument] string Zone
) : GcloudOptions;