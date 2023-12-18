using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ams", "transform", "create")]
public record AzAmsTransformCreateOptions(
[property: CommandSwitch("--account-name")] int AccountName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--preset")] string Preset,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--audio-analysis-mode")]
    public string? AudioAnalysisMode { get; set; }

    [CommandSwitch("--audio-language")]
    public string? AudioLanguage { get; set; }

    [CommandSwitch("--blur-type")]
    public string? BlurType { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--face-detector-mode")]
    public string? FaceDetectorMode { get; set; }

    [CommandSwitch("--insights-to-extract")]
    public string? InsightsToExtract { get; set; }

    [CommandSwitch("--on-error")]
    public string? OnError { get; set; }

    [CommandSwitch("--relative-priority")]
    public string? RelativePriority { get; set; }

    [CommandSwitch("--resolution")]
    public string? Resolution { get; set; }

    [CommandSwitch("--video-analysis-mode")]
    public string? VideoAnalysisMode { get; set; }
}