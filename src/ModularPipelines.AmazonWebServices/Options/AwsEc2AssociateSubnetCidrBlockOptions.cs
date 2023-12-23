using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "associate-subnet-cidr-block")]
public record AwsEc2AssociateSubnetCidrBlockOptions(
[property: CommandSwitch("--subnet-id")] string SubnetId
) : AwsOptions
{
    [CommandSwitch("--ipv6-cidr-block")]
    public string? Ipv6CidrBlock { get; set; }

    [CommandSwitch("--ipv6-ipam-pool-id")]
    public string? Ipv6IpamPoolId { get; set; }

    [CommandSwitch("--ipv6-netmask-length")]
    public int? Ipv6NetmaskLength { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}