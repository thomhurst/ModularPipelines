using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("redis", "instances", "upgrade")]
public record GcloudRedisInstancesUpgradeOptions(
[property: CliArgument] string Instance,
[property: CliArgument] string Region,
[property: CliOption("--redis-version")] string RedisVersion
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }
}