using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cognito-sync", "list-records")]
public record AwsCognitoSyncListRecordsOptions(
[property: CliOption("--identity-pool-id")] string IdentityPoolId,
[property: CliOption("--identity-id")] string IdentityId,
[property: CliOption("--dataset-name")] string DatasetName
) : AwsOptions
{
    [CliOption("--last-sync-count")]
    public long? LastSyncCount { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--sync-session-token")]
    public string? SyncSessionToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}