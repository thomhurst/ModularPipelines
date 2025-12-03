using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mediatailor", "put-playback-configuration")]
public record AwsMediatailorPutPlaybackConfigurationOptions(
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--ad-decision-server-url")]
    public string? AdDecisionServerUrl { get; set; }

    [CliOption("--avail-suppression")]
    public string? AvailSuppression { get; set; }

    [CliOption("--bumper")]
    public string? Bumper { get; set; }

    [CliOption("--cdn-configuration")]
    public string? CdnConfiguration { get; set; }

    [CliOption("--configuration-aliases")]
    public IEnumerable<KeyValue>? ConfigurationAliases { get; set; }

    [CliOption("--dash-configuration")]
    public string? DashConfiguration { get; set; }

    [CliOption("--live-pre-roll-configuration")]
    public string? LivePreRollConfiguration { get; set; }

    [CliOption("--manifest-processing-rules")]
    public string? ManifestProcessingRules { get; set; }

    [CliOption("--personalization-threshold-seconds")]
    public int? PersonalizationThresholdSeconds { get; set; }

    [CliOption("--slate-ad-url")]
    public string? SlateAdUrl { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--transcode-profile-name")]
    public string? TranscodeProfileName { get; set; }

    [CliOption("--video-content-source-url")]
    public string? VideoContentSourceUrl { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}