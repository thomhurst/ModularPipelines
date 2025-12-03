using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "url-maps", "import")]
public record GcloudComputeUrlMapsImportOptions(
[property: CliArgument] string UrlMap
) : GcloudOptions
{
    [CliOption("--source")]
    public string? Source { get; set; }

    [CliFlag("--global")]
    public bool? Global { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }
}