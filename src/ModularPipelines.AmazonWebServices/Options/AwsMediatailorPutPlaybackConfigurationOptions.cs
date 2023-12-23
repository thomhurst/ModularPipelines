using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mediatailor", "put-playback-configuration")]
public record AwsMediatailorPutPlaybackConfigurationOptions(
[property: CommandSwitch("--name")] string Name
) : AwsOptions
{
    [CommandSwitch("--ad-decision-server-url")]
    public string? AdDecisionServerUrl { get; set; }

    [CommandSwitch("--avail-suppression")]
    public string? AvailSuppression { get; set; }

    [CommandSwitch("--bumper")]
    public string? Bumper { get; set; }

    [CommandSwitch("--cdn-configuration")]
    public string? CdnConfiguration { get; set; }

    [CommandSwitch("--configuration-aliases")]
    public IEnumerable<KeyValue>? ConfigurationAliases { get; set; }

    [CommandSwitch("--dash-configuration")]
    public string? DashConfiguration { get; set; }

    [CommandSwitch("--live-pre-roll-configuration")]
    public string? LivePreRollConfiguration { get; set; }

    [CommandSwitch("--manifest-processing-rules")]
    public string? ManifestProcessingRules { get; set; }

    [CommandSwitch("--personalization-threshold-seconds")]
    public int? PersonalizationThresholdSeconds { get; set; }

    [CommandSwitch("--slate-ad-url")]
    public string? SlateAdUrl { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--transcode-profile-name")]
    public string? TranscodeProfileName { get; set; }

    [CommandSwitch("--video-content-source-url")]
    public string? VideoContentSourceUrl { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}