using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("memcache", "instances", "describe")]
public record GcloudMemcacheInstancesDescribeOptions(
[property: CliArgument] string Instance,
[property: CliArgument] string Region
) : GcloudOptions;