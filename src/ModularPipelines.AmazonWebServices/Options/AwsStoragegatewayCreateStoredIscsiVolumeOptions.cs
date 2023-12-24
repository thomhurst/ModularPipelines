using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storagegateway", "create-stored-iscsi-volume")]
public record AwsStoragegatewayCreateStoredIscsiVolumeOptions(
[property: CommandSwitch("--gateway-arn")] string GatewayArn,
[property: CommandSwitch("--disk-id")] string DiskId,
[property: CommandSwitch("--target-name")] string TargetName,
[property: CommandSwitch("--network-interface-id")] string NetworkInterfaceId
) : AwsOptions
{
    [CommandSwitch("--snapshot-id")]
    public string? SnapshotId { get; set; }

    [CommandSwitch("--kms-key")]
    public string? KmsKey { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}