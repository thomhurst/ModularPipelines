using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "associate-subnet-cidr-block")]
public record AwsEc2AssociateSubnetCidrBlockOptions(
[property: CliOption("--subnet-id")] string SubnetId
) : AwsOptions
{
    [CliOption("--ipv6-cidr-block")]
    public string? Ipv6CidrBlock { get; set; }

    [CliOption("--ipv6-ipam-pool-id")]
    public string? Ipv6IpamPoolId { get; set; }

    [CliOption("--ipv6-netmask-length")]
    public int? Ipv6NetmaskLength { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}