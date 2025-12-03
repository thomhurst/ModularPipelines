using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "url-maps", "invalidate-cdn-cache")]
public record GcloudComputeUrlMapsInvalidateCdnCacheOptions(
[property: CliArgument] string Urlmap,
[property: CliOption("--path")] string Path
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--host")]
    public string? Host { get; set; }

    [CliFlag("--global")]
    public bool? Global { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }
}