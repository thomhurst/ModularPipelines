using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ml", "vision", "detect-web")]
public record GcloudMlVisionDetectWebOptions(
[property: CliArgument] string ImagePath
) : GcloudOptions
{
    [CliOption("--max-results")]
    public string? MaxResults { get; set; }
}