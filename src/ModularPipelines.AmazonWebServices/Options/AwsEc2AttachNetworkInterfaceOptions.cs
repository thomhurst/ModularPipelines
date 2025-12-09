using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "attach-network-interface")]
public record AwsEc2AttachNetworkInterfaceOptions(
[property: CliOption("--device-index")] int DeviceIndex,
[property: CliOption("--instance-id")] string InstanceId,
[property: CliOption("--network-interface-id")] string NetworkInterfaceId
) : AwsOptions
{
    [CliOption("--network-card-index")]
    public int? NetworkCardIndex { get; set; }

    [CliOption("--ena-srd-specification")]
    public string? EnaSrdSpecification { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}