using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("memcache", "operations", "describe")]
public record GcloudMemcacheOperationsDescribeOptions(
[property: CliArgument] string Operation,
[property: CliArgument] string Region
) : GcloudOptions;