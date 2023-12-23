using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ivs-realtime", "get-participant")]
public record AwsIvsRealtimeGetParticipantOptions(
[property: CommandSwitch("--participant-id")] string ParticipantId,
[property: CommandSwitch("--session-id")] string SessionId,
[property: CommandSwitch("--stage-arn")] string StageArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}