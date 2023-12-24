using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cognito-sync", "update-records")]
public record AwsCognitoSyncUpdateRecordsOptions(
[property: CommandSwitch("--identity-pool-id")] string IdentityPoolId,
[property: CommandSwitch("--identity-id")] string IdentityId,
[property: CommandSwitch("--dataset-name")] string DatasetName,
[property: CommandSwitch("--sync-session-token")] string SyncSessionToken
) : AwsOptions
{
    [CommandSwitch("--device-id")]
    public string? DeviceId { get; set; }

    [CommandSwitch("--record-patches")]
    public string[]? RecordPatches { get; set; }

    [CommandSwitch("--client-context")]
    public string? ClientContext { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}