using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ml", "vision", "detect-faces")]
public record GcloudMlVisionDetectFacesOptions(
[property: CliArgument] string ImagePath
) : GcloudOptions
{
    [CliOption("--max-results")]
    public string? MaxResults { get; set; }
}