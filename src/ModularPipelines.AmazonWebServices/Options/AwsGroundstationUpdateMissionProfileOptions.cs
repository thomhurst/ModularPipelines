using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("groundstation", "update-mission-profile")]
public record AwsGroundstationUpdateMissionProfileOptions(
[property: CliOption("--mission-profile-id")] string MissionProfileId
) : AwsOptions
{
    [CliOption("--contact-post-pass-duration-seconds")]
    public int? ContactPostPassDurationSeconds { get; set; }

    [CliOption("--contact-pre-pass-duration-seconds")]
    public int? ContactPrePassDurationSeconds { get; set; }

    [CliOption("--dataflow-edges")]
    public string[]? DataflowEdges { get; set; }

    [CliOption("--minimum-viable-contact-duration-seconds")]
    public int? MinimumViableContactDurationSeconds { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--streams-kms-key")]
    public string? StreamsKmsKey { get; set; }

    [CliOption("--streams-kms-role")]
    public string? StreamsKmsRole { get; set; }

    [CliOption("--tracking-config-arn")]
    public string? TrackingConfigArn { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}