using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("edge-cloud", "container", "zones", "describe")]
public record GcloudEdgeCloudContainerZonesDescribeOptions(
[property: CliArgument] string Zone
) : GcloudOptions;