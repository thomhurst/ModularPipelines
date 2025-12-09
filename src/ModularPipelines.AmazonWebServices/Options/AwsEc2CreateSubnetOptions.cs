using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "create-subnet")]
public record AwsEc2CreateSubnetOptions(
[property: CliOption("--vpc-id")] string VpcId
) : AwsOptions
{
    [CliOption("--tag-specifications")]
    public string[]? TagSpecifications { get; set; }

    [CliOption("--availability-zone")]
    public string? AvailabilityZone { get; set; }

    [CliOption("--availability-zone-id")]
    public string? AvailabilityZoneId { get; set; }

    [CliOption("--cidr-block")]
    public string? CidrBlock { get; set; }

    [CliOption("--ipv6-cidr-block")]
    public string? Ipv6CidrBlock { get; set; }

    [CliOption("--outpost-arn")]
    public string? OutpostArn { get; set; }

    [CliOption("--ipv4-ipam-pool-id")]
    public string? Ipv4IpamPoolId { get; set; }

    [CliOption("--ipv4-netmask-length")]
    public int? Ipv4NetmaskLength { get; set; }

    [CliOption("--ipv6-ipam-pool-id")]
    public string? Ipv6IpamPoolId { get; set; }

    [CliOption("--ipv6-netmask-length")]
    public int? Ipv6NetmaskLength { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}