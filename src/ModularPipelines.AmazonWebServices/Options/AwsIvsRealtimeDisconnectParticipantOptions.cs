using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ivs-realtime", "disconnect-participant")]
public record AwsIvsRealtimeDisconnectParticipantOptions(
[property: CommandSwitch("--participant-id")] string ParticipantId,
[property: CommandSwitch("--stage-arn")] string StageArn
) : AwsOptions
{
    [CommandSwitch("--reason")]
    public string? Reason { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}