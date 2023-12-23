using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "modify-subnet-attribute")]
public record AwsEc2ModifySubnetAttributeOptions(
[property: CommandSwitch("--subnet-id")] string SubnetId
) : AwsOptions
{
    [CommandSwitch("--customer-owned-ipv4-pool")]
    public string? CustomerOwnedIpv4Pool { get; set; }

    [CommandSwitch("--private-dns-hostname-type-on-launch")]
    public string? PrivateDnsHostnameTypeOnLaunch { get; set; }

    [CommandSwitch("--enable-lni-at-device-index")]
    public int? EnableLniAtDeviceIndex { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}