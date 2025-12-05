using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ml", "video", "detect-labels")]
public record GcloudMlVideoDetectLabelsOptions(
[property: CliArgument] string InputPath
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--detection-mode")]
    public string? DetectionMode { get; set; }

    [CliOption("--output-uri")]
    public string? OutputUri { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }

    [CliOption("--segments")]
    public string[]? Segments { get; set; }
}