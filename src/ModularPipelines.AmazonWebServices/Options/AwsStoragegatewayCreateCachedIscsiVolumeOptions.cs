using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("storagegateway", "create-cached-iscsi-volume")]
public record AwsStoragegatewayCreateCachedIscsiVolumeOptions(
[property: CliOption("--gateway-arn")] string GatewayArn,
[property: CliOption("--volume-size-in-bytes")] long VolumeSizeInBytes,
[property: CliOption("--target-name")] string TargetName,
[property: CliOption("--network-interface-id")] string NetworkInterfaceId,
[property: CliOption("--client-token")] string ClientToken
) : AwsOptions
{
    [CliOption("--snapshot-id")]
    public string? SnapshotId { get; set; }

    [CliOption("--source-volume-arn")]
    public string? SourceVolumeArn { get; set; }

    [CliOption("--kms-key")]
    public string? KmsKey { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}