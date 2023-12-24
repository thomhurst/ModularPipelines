using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("groundstation", "create-mission-profile")]
public record AwsGroundstationCreateMissionProfileOptions(
[property: CommandSwitch("--dataflow-edges")] string[] DataflowEdges,
[property: CommandSwitch("--minimum-viable-contact-duration-seconds")] int MinimumViableContactDurationSeconds,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--tracking-config-arn")] string TrackingConfigArn
) : AwsOptions
{
    [CommandSwitch("--contact-post-pass-duration-seconds")]
    public int? ContactPostPassDurationSeconds { get; set; }

    [CommandSwitch("--contact-pre-pass-duration-seconds")]
    public int? ContactPrePassDurationSeconds { get; set; }

    [CommandSwitch("--streams-kms-key")]
    public string? StreamsKmsKey { get; set; }

    [CommandSwitch("--streams-kms-role")]
    public string? StreamsKmsRole { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}