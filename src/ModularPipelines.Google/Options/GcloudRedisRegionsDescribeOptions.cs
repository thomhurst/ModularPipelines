using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("redis", "regions", "describe")]
public record GcloudRedisRegionsDescribeOptions(
[property: CliArgument] string Region
) : GcloudOptions;