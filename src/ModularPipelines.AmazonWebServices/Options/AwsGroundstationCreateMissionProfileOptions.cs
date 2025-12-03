using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("groundstation", "create-mission-profile")]
public record AwsGroundstationCreateMissionProfileOptions(
[property: CliOption("--dataflow-edges")] string[] DataflowEdges,
[property: CliOption("--minimum-viable-contact-duration-seconds")] int MinimumViableContactDurationSeconds,
[property: CliOption("--name")] string Name,
[property: CliOption("--tracking-config-arn")] string TrackingConfigArn
) : AwsOptions
{
    [CliOption("--contact-post-pass-duration-seconds")]
    public int? ContactPostPassDurationSeconds { get; set; }

    [CliOption("--contact-pre-pass-duration-seconds")]
    public int? ContactPrePassDurationSeconds { get; set; }

    [CliOption("--streams-kms-key")]
    public string? StreamsKmsKey { get; set; }

    [CliOption("--streams-kms-role")]
    public string? StreamsKmsRole { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}