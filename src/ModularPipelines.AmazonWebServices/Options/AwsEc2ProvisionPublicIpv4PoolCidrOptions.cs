using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "provision-public-ipv4-pool-cidr")]
public record AwsEc2ProvisionPublicIpv4PoolCidrOptions(
[property: CliOption("--ipam-pool-id")] string IpamPoolId,
[property: CliOption("--pool-id")] string PoolId,
[property: CliOption("--netmask-length")] int NetmaskLength
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}