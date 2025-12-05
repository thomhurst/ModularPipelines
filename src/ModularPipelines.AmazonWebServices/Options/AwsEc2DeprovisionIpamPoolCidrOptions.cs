using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "deprovision-ipam-pool-cidr")]
public record AwsEc2DeprovisionIpamPoolCidrOptions(
[property: CliOption("--ipam-pool-id")] string IpamPoolId
) : AwsOptions
{
    [CliOption("--cidr")]
    public string? Cidr { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}