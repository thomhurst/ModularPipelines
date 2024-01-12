using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("edge-cloud", "container", "zones", "describe")]
public record GcloudEdgeCloudContainerZonesDescribeOptions(
[property: PositionalArgument] string Zone
) : GcloudOptions;