using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("edge-cache", "origins", "describe")]
public record GcloudEdgeCacheOriginsDescribeOptions(
[property: CliArgument] string Origin,
[property: CliArgument] string Location
) : GcloudOptions;