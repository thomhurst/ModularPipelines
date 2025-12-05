using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ivs-realtime", "list-participant-events")]
public record AwsIvsRealtimeListParticipantEventsOptions(
[property: CliOption("--participant-id")] string ParticipantId,
[property: CliOption("--session-id")] string SessionId,
[property: CliOption("--stage-arn")] string StageArn
) : AwsOptions
{
    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}