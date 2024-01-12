using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("edge-cache", "operations", "describe")]
public record GcloudEdgeCacheOperationsDescribeOptions(
[property: PositionalArgument] string Operation
) : GcloudOptions;