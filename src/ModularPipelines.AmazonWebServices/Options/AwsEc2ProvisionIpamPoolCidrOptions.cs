using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "provision-ipam-pool-cidr")]
public record AwsEc2ProvisionIpamPoolCidrOptions(
[property: CliOption("--ipam-pool-id")] string IpamPoolId
) : AwsOptions
{
    [CliOption("--cidr")]
    public string? Cidr { get; set; }

    [CliOption("--cidr-authorization-context")]
    public string? CidrAuthorizationContext { get; set; }

    [CliOption("--netmask-length")]
    public int? NetmaskLength { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}