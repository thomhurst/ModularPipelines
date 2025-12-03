using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ams", "transform", "output", "add")]
public record AzAmsTransformOutputAddOptions(
[property: CliOption("--preset")] string Preset
) : AzOptions
{
    [CliOption("--account-name")]
    public int? AccountName { get; set; }

    [CliOption("--audio-analysis-mode")]
    public string? AudioAnalysisMode { get; set; }

    [CliOption("--audio-language")]
    public string? AudioLanguage { get; set; }

    [CliOption("--blur-type")]
    public string? BlurType { get; set; }

    [CliOption("--face-detector-mode")]
    public string? FaceDetectorMode { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--insights-to-extract")]
    public string? InsightsToExtract { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--on-error")]
    public string? OnError { get; set; }

    [CliOption("--relative-priority")]
    public string? RelativePriority { get; set; }

    [CliOption("--resolution")]
    public string? Resolution { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--video-analysis-mode")]
    public string? VideoAnalysisMode { get; set; }
}