using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ivs-realtime", "disconnect-participant")]
public record AwsIvsRealtimeDisconnectParticipantOptions(
[property: CliOption("--participant-id")] string ParticipantId,
[property: CliOption("--stage-arn")] string StageArn
) : AwsOptions
{
    [CliOption("--reason")]
    public string? Reason { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}