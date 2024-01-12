using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "url-maps", "list-cdn-cache-invalidations")]
public record GcloudComputeUrlMapsListCdnCacheInvalidationsOptions(
[property: PositionalArgument] string UrlMap
) : GcloudOptions
{
    [CommandSwitch("--limit")]
    public string? Limit { get; set; }

    [BooleanCommandSwitch("--global")]
    public bool? Global { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }
}