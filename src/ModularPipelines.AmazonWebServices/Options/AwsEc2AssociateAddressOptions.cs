using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "associate-address")]
public record AwsEc2AssociateAddressOptions : AwsOptions
{
    [CommandSwitch("--allocation-id")]
    public string? AllocationId { get; set; }

    [CommandSwitch("--instance-id")]
    public string? InstanceId { get; set; }

    [CommandSwitch("--public-ip")]
    public string? PublicIp { get; set; }

    [CommandSwitch("--network-interface-id")]
    public string? NetworkInterfaceId { get; set; }

    [CommandSwitch("--private-ip-address")]
    public string? PrivateIpAddress { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}