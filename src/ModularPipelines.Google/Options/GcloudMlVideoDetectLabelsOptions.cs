using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ml", "video", "detect-labels")]
public record GcloudMlVideoDetectLabelsOptions(
[property: PositionalArgument] string InputPath
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--detection-mode")]
    public string? DetectionMode { get; set; }

    [CommandSwitch("--output-uri")]
    public string? OutputUri { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }

    [CommandSwitch("--segments")]
    public string[]? Segments { get; set; }
}