using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ivs-realtime", "get-stage-session")]
public record AwsIvsRealtimeGetStageSessionOptions(
[property: CliOption("--session-id")] string SessionId,
[property: CliOption("--stage-arn")] string StageArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}