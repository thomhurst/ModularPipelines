using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "create-subnet")]
public record AwsEc2CreateSubnetOptions(
[property: CommandSwitch("--vpc-id")] string VpcId
) : AwsOptions
{
    [CommandSwitch("--tag-specifications")]
    public string[]? TagSpecifications { get; set; }

    [CommandSwitch("--availability-zone")]
    public string? AvailabilityZone { get; set; }

    [CommandSwitch("--availability-zone-id")]
    public string? AvailabilityZoneId { get; set; }

    [CommandSwitch("--cidr-block")]
    public string? CidrBlock { get; set; }

    [CommandSwitch("--ipv6-cidr-block")]
    public string? Ipv6CidrBlock { get; set; }

    [CommandSwitch("--outpost-arn")]
    public string? OutpostArn { get; set; }

    [CommandSwitch("--ipv4-ipam-pool-id")]
    public string? Ipv4IpamPoolId { get; set; }

    [CommandSwitch("--ipv4-netmask-length")]
    public int? Ipv4NetmaskLength { get; set; }

    [CommandSwitch("--ipv6-ipam-pool-id")]
    public string? Ipv6IpamPoolId { get; set; }

    [CommandSwitch("--ipv6-netmask-length")]
    public int? Ipv6NetmaskLength { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}