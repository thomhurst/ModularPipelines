using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("redis", "operations", "describe")]
public record GcloudRedisOperationsDescribeOptions(
[property: CliArgument] string Operation,
[property: CliArgument] string Region
) : GcloudOptions;