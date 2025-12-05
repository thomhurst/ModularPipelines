using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("storagegateway", "attach-volume")]
public record AwsStoragegatewayAttachVolumeOptions(
[property: CliOption("--gateway-arn")] string GatewayArn,
[property: CliOption("--volume-arn")] string VolumeArn,
[property: CliOption("--network-interface-id")] string NetworkInterfaceId
) : AwsOptions
{
    [CliOption("--target-name")]
    public string? TargetName { get; set; }

    [CliOption("--disk-id")]
    public string? DiskId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}