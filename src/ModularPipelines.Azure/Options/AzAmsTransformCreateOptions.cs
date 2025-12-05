using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("ams", "transform", "create")]
public record AzAmsTransformCreateOptions(
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--name")] string Name,
[property: CliOption("--preset")] string Preset,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--audio-analysis-mode")]
    public string? AudioAnalysisMode { get; set; }

    [CliOption("--audio-language")]
    public string? AudioLanguage { get; set; }

    [CliOption("--blur-type")]
    public string? BlurType { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--face-detector-mode")]
    public string? FaceDetectorMode { get; set; }

    [CliOption("--insights-to-extract")]
    public string? InsightsToExtract { get; set; }

    [CliOption("--on-error")]
    public string? OnError { get; set; }

    [CliOption("--relative-priority")]
    public string? RelativePriority { get; set; }

    [CliOption("--resolution")]
    public string? Resolution { get; set; }

    [CliOption("--video-analysis-mode")]
    public string? VideoAnalysisMode { get; set; }
}