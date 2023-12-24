using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ivs-realtime", "list-participant-events")]
public record AwsIvsRealtimeListParticipantEventsOptions(
[property: CommandSwitch("--participant-id")] string ParticipantId,
[property: CommandSwitch("--session-id")] string SessionId,
[property: CommandSwitch("--stage-arn")] string StageArn
) : AwsOptions
{
    [CommandSwitch("--max-results")]
    public int? MaxResults { get; set; }

    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}