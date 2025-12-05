using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("storagegateway", "create-stored-iscsi-volume")]
public record AwsStoragegatewayCreateStoredIscsiVolumeOptions(
[property: CliOption("--gateway-arn")] string GatewayArn,
[property: CliOption("--disk-id")] string DiskId,
[property: CliOption("--target-name")] string TargetName,
[property: CliOption("--network-interface-id")] string NetworkInterfaceId
) : AwsOptions
{
    [CliOption("--snapshot-id")]
    public string? SnapshotId { get; set; }

    [CliOption("--kms-key")]
    public string? KmsKey { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}