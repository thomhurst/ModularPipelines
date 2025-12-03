using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("edge-cache", "operations", "describe")]
public record GcloudEdgeCacheOperationsDescribeOptions(
[property: CliArgument] string Operation
) : GcloudOptions;