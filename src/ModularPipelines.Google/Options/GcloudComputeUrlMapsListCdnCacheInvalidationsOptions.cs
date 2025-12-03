using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "url-maps", "list-cdn-cache-invalidations")]
public record GcloudComputeUrlMapsListCdnCacheInvalidationsOptions(
[property: CliArgument] string UrlMap
) : GcloudOptions
{
    [CliOption("--limit")]
    public string? Limit { get; set; }

    [CliFlag("--global")]
    public bool? Global { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }
}