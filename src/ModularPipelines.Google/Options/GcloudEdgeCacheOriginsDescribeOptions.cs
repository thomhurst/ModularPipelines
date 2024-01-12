using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("edge-cache", "origins", "describe")]
public record GcloudEdgeCacheOriginsDescribeOptions(
[property: PositionalArgument] string Origin,
[property: PositionalArgument] string Location
) : GcloudOptions;