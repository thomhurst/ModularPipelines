using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "url-maps", "export")]
public record GcloudComputeUrlMapsExportOptions(
[property: CliArgument] string UrlMap
) : GcloudOptions
{
    [CliOption("--destination")]
    public string? Destination { get; set; }

    [CliFlag("--global")]
    public bool? Global { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }
}