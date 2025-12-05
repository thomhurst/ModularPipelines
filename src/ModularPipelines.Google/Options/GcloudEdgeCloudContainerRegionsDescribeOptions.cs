using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("edge-cloud", "container", "regions", "describe")]
public record GcloudEdgeCloudContainerRegionsDescribeOptions(
[property: CliArgument] string Location
) : GcloudOptions;