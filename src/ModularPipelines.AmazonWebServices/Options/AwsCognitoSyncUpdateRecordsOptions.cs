using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cognito-sync", "update-records")]
public record AwsCognitoSyncUpdateRecordsOptions(
[property: CliOption("--identity-pool-id")] string IdentityPoolId,
[property: CliOption("--identity-id")] string IdentityId,
[property: CliOption("--dataset-name")] string DatasetName,
[property: CliOption("--sync-session-token")] string SyncSessionToken
) : AwsOptions
{
    [CliOption("--device-id")]
    public string? DeviceId { get; set; }

    [CliOption("--record-patches")]
    public string[]? RecordPatches { get; set; }

    [CliOption("--client-context")]
    public string? ClientContext { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}