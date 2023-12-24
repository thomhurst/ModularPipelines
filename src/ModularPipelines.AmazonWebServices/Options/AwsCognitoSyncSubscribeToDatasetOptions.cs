using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cognito-sync", "subscribe-to-dataset")]
public record AwsCognitoSyncSubscribeToDatasetOptions(
[property: CommandSwitch("--identity-pool-id")] string IdentityPoolId,
[property: CommandSwitch("--identity-id")] string IdentityId,
[property: CommandSwitch("--dataset-name")] string DatasetName,
[property: CommandSwitch("--device-id")] string DeviceId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}