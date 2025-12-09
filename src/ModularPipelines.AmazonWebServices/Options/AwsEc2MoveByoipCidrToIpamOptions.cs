using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "move-byoip-cidr-to-ipam")]
public record AwsEc2MoveByoipCidrToIpamOptions(
[property: CliOption("--cidr")] string Cidr,
[property: CliOption("--ipam-pool-id")] string IpamPoolId,
[property: CliOption("--ipam-pool-owner")] string IpamPoolOwner
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}