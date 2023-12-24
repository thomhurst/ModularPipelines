using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ivs-realtime", "get-stage-session")]
public record AwsIvsRealtimeGetStageSessionOptions(
[property: CommandSwitch("--session-id")] string SessionId,
[property: CommandSwitch("--stage-arn")] string StageArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}