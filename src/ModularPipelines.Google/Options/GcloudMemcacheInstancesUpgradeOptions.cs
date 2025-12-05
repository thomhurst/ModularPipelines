using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("memcache", "instances", "upgrade")]
public record GcloudMemcacheInstancesUpgradeOptions(
[property: CliArgument] string Instance,
[property: CliArgument] string Region,
[property: CliOption("--memcached-version")] string MemcachedVersion
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }
}