using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "allocate-ipam-pool-cidr")]
public record AwsEc2AllocateIpamPoolCidrOptions(
[property: CliOption("--ipam-pool-id")] string IpamPoolId
) : AwsOptions
{
    [CliOption("--cidr")]
    public string? Cidr { get; set; }

    [CliOption("--netmask-length")]
    public int? NetmaskLength { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--allowed-cidrs")]
    public string[]? AllowedCidrs { get; set; }

    [CliOption("--disallowed-cidrs")]
    public string[]? DisallowedCidrs { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}