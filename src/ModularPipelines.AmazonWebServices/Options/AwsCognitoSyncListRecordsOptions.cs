using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cognito-sync", "list-records")]
public record AwsCognitoSyncListRecordsOptions(
[property: CommandSwitch("--identity-pool-id")] string IdentityPoolId,
[property: CommandSwitch("--identity-id")] string IdentityId,
[property: CommandSwitch("--dataset-name")] string DatasetName
) : AwsOptions
{
    [CommandSwitch("--last-sync-count")]
    public long? LastSyncCount { get; set; }

    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--max-results")]
    public int? MaxResults { get; set; }

    [CommandSwitch("--sync-session-token")]
    public string? SyncSessionToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}