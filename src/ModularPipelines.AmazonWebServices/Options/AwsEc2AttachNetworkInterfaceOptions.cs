using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "attach-network-interface")]
public record AwsEc2AttachNetworkInterfaceOptions(
[property: CommandSwitch("--device-index")] int DeviceIndex,
[property: CommandSwitch("--instance-id")] string InstanceId,
[property: CommandSwitch("--network-interface-id")] string NetworkInterfaceId
) : AwsOptions
{
    [CommandSwitch("--network-card-index")]
    public int? NetworkCardIndex { get; set; }

    [CommandSwitch("--ena-srd-specification")]
    public string? EnaSrdSpecification { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}