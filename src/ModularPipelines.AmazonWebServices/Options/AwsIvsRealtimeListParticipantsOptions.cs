using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ivs-realtime", "list-participants")]
public record AwsIvsRealtimeListParticipantsOptions(
[property: CliOption("--session-id")] string SessionId,
[property: CliOption("--stage-arn")] string StageArn
) : AwsOptions
{
    [CliOption("--filter-by-state")]
    public string? FilterByState { get; set; }

    [CliOption("--filter-by-user-id")]
    public string? FilterByUserId { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}