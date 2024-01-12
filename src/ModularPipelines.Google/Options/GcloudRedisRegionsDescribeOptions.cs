using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("redis", "regions", "describe")]
public record GcloudRedisRegionsDescribeOptions(
[property: PositionalArgument] string Region
) : GcloudOptions;