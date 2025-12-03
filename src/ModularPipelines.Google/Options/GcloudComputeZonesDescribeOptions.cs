using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "zones", "describe")]
public record GcloudComputeZonesDescribeOptions(
[property: CliArgument] string Name
) : GcloudOptions;