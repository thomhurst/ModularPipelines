using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storagegateway", "create-cached-iscsi-volume")]
public record AwsStoragegatewayCreateCachedIscsiVolumeOptions(
[property: CommandSwitch("--gateway-arn")] string GatewayArn,
[property: CommandSwitch("--volume-size-in-bytes")] long VolumeSizeInBytes,
[property: CommandSwitch("--target-name")] string TargetName,
[property: CommandSwitch("--network-interface-id")] string NetworkInterfaceId,
[property: CommandSwitch("--client-token")] string ClientToken
) : AwsOptions
{
    [CommandSwitch("--snapshot-id")]
    public string? SnapshotId { get; set; }

    [CommandSwitch("--source-volume-arn")]
    public string? SourceVolumeArn { get; set; }

    [CommandSwitch("--kms-key")]
    public string? KmsKey { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}