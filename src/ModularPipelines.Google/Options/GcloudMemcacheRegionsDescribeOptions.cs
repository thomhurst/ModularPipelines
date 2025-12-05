using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("memcache", "regions", "describe")]
public record GcloudMemcacheRegionsDescribeOptions(
[property: CliArgument] string Region
) : GcloudOptions;