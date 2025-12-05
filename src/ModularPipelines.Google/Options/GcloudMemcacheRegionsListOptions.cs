using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("memcache", "regions", "list")]
public record GcloudMemcacheRegionsListOptions : GcloudOptions;