using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("groundstation", "update-mission-profile")]
public record AwsGroundstationUpdateMissionProfileOptions(
[property: CommandSwitch("--mission-profile-id")] string MissionProfileId
) : AwsOptions
{
    [CommandSwitch("--contact-post-pass-duration-seconds")]
    public int? ContactPostPassDurationSeconds { get; set; }

    [CommandSwitch("--contact-pre-pass-duration-seconds")]
    public int? ContactPrePassDurationSeconds { get; set; }

    [CommandSwitch("--dataflow-edges")]
    public string[]? DataflowEdges { get; set; }

    [CommandSwitch("--minimum-viable-contact-duration-seconds")]
    public int? MinimumViableContactDurationSeconds { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--streams-kms-key")]
    public string? StreamsKmsKey { get; set; }

    [CommandSwitch("--streams-kms-role")]
    public string? StreamsKmsRole { get; set; }

    [CommandSwitch("--tracking-config-arn")]
    public string? TrackingConfigArn { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}