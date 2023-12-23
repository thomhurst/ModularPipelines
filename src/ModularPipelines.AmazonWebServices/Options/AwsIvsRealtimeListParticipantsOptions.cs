using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ivs-realtime", "list-participants")]
public record AwsIvsRealtimeListParticipantsOptions(
[property: CommandSwitch("--session-id")] string SessionId,
[property: CommandSwitch("--stage-arn")] string StageArn
) : AwsOptions
{
    [CommandSwitch("--filter-by-state")]
    public string? FilterByState { get; set; }

    [CommandSwitch("--filter-by-user-id")]
    public string? FilterByUserId { get; set; }

    [CommandSwitch("--max-results")]
    public int? MaxResults { get; set; }

    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}