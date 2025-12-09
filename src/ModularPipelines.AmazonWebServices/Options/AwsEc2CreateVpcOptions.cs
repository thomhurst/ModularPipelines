using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "create-vpc")]
public record AwsEc2CreateVpcOptions : AwsOptions
{
    [CliOption("--cidr-block")]
    public string? CidrBlock { get; set; }

    [CliOption("--ipv6-pool")]
    public string? Ipv6Pool { get; set; }

    [CliOption("--ipv6-cidr-block")]
    public string? Ipv6CidrBlock { get; set; }

    [CliOption("--ipv4-ipam-pool-id")]
    public string? Ipv4IpamPoolId { get; set; }

    [CliOption("--ipv4-netmask-length")]
    public int? Ipv4NetmaskLength { get; set; }

    [CliOption("--ipv6-ipam-pool-id")]
    public string? Ipv6IpamPoolId { get; set; }

    [CliOption("--ipv6-netmask-length")]
    public int? Ipv6NetmaskLength { get; set; }

    [CliOption("--instance-tenancy")]
    public string? InstanceTenancy { get; set; }

    [CliOption("--ipv6-cidr-block-network-border-group")]
    public string? Ipv6CidrBlockNetworkBorderGroup { get; set; }

    [CliOption("--tag-specifications")]
    public string[]? TagSpecifications { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}