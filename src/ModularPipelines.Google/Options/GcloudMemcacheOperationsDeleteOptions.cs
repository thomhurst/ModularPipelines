using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("memcache", "operations", "delete")]
public record GcloudMemcacheOperationsDeleteOptions(
[property: CliArgument] string Operation,
[property: CliArgument] string Region
) : GcloudOptions;