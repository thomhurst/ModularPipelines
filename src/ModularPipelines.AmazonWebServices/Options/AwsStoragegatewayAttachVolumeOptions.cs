using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storagegateway", "attach-volume")]
public record AwsStoragegatewayAttachVolumeOptions(
[property: CommandSwitch("--gateway-arn")] string GatewayArn,
[property: CommandSwitch("--volume-arn")] string VolumeArn,
[property: CommandSwitch("--network-interface-id")] string NetworkInterfaceId
) : AwsOptions
{
    [CommandSwitch("--target-name")]
    public string? TargetName { get; set; }

    [CommandSwitch("--disk-id")]
    public string? DiskId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}