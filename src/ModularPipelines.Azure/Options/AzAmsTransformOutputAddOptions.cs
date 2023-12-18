using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ams", "transform", "output", "add")]
public record AzAmsTransformOutputAddOptions(
[property: CommandSwitch("--preset")] string Preset
) : AzOptions
{
    [CommandSwitch("--account-name")]
    public int? AccountName { get; set; }

    [CommandSwitch("--audio-analysis-mode")]
    public string? AudioAnalysisMode { get; set; }

    [CommandSwitch("--audio-language")]
    public string? AudioLanguage { get; set; }

    [CommandSwitch("--blur-type")]
    public string? BlurType { get; set; }

    [CommandSwitch("--face-detector-mode")]
    public string? FaceDetectorMode { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--insights-to-extract")]
    public string? InsightsToExtract { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--on-error")]
    public string? OnError { get; set; }

    [CommandSwitch("--relative-priority")]
    public string? RelativePriority { get; set; }

    [CommandSwitch("--resolution")]
    public string? Resolution { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [CommandSwitch("--video-analysis-mode")]
    public string? VideoAnalysisMode { get; set; }
}

