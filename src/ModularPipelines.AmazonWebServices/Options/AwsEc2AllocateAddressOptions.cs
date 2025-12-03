using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "allocate-address")]
public record AwsEc2AllocateAddressOptions : AwsOptions
{
    [CliOption("--domain")]
    public string? Domain { get; set; }

    [CliOption("--address")]
    public string? Address { get; set; }

    [CliOption("--public-ipv4-pool")]
    public string? PublicIpv4Pool { get; set; }

    [CliOption("--network-border-group")]
    public string? NetworkBorderGroup { get; set; }

    [CliOption("--customer-owned-ipv4-pool")]
    public string? CustomerOwnedIpv4Pool { get; set; }

    [CliOption("--tag-specifications")]
    public string[]? TagSpecifications { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}