using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("redis", "instances", "describe")]
public record GcloudRedisInstancesDescribeOptions(
[property: CliArgument] string Instance,
[property: CliArgument] string Region
) : GcloudOptions;