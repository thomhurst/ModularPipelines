using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "release-ipam-pool-allocation")]
public record AwsEc2ReleaseIpamPoolAllocationOptions(
[property: CliOption("--ipam-pool-id")] string IpamPoolId,
[property: CliOption("--cidr")] string Cidr,
[property: CliOption("--ipam-pool-allocation-id")] string IpamPoolAllocationId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}