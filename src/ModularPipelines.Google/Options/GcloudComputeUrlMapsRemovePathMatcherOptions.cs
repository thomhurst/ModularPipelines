using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "url-maps", "remove-path-matcher")]
public record GcloudComputeUrlMapsRemovePathMatcherOptions(
[property: CliArgument] string UrlMap,
[property: CliOption("--path-matcher-name")] string PathMatcherName
) : GcloudOptions
{
    [CliFlag("--global")]
    public bool? Global { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }
}