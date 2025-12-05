using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ivs-realtime", "get-participant")]
public record AwsIvsRealtimeGetParticipantOptions(
[property: CliOption("--participant-id")] string ParticipantId,
[property: CliOption("--session-id")] string SessionId,
[property: CliOption("--stage-arn")] string StageArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}