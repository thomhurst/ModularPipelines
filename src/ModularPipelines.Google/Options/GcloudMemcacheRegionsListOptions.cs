using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("memcache", "regions", "list")]
public record GcloudMemcacheRegionsListOptions : GcloudOptions;