using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("edge-cloud", "container", "regions", "describe")]
public record GcloudEdgeCloudContainerRegionsDescribeOptions(
[property: PositionalArgument] string Location
) : GcloudOptions;