using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "deprovision-public-ipv4-pool-cidr")]
public record AwsEc2DeprovisionPublicIpv4PoolCidrOptions(
[property: CliOption("--pool-id")] string PoolId,
[property: CliOption("--cidr")] string Cidr
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}