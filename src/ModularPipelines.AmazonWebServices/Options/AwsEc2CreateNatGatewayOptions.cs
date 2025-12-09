using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "create-nat-gateway")]
public record AwsEc2CreateNatGatewayOptions(
[property: CliOption("--subnet-id")] string SubnetId
) : AwsOptions
{
    [CliOption("--allocation-id")]
    public string? AllocationId { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--tag-specifications")]
    public string[]? TagSpecifications { get; set; }

    [CliOption("--connectivity-type")]
    public string? ConnectivityType { get; set; }

    [CliOption("--private-ip-address")]
    public string? PrivateIpAddress { get; set; }

    [CliOption("--secondary-allocation-ids")]
    public string[]? SecondaryAllocationIds { get; set; }

    [CliOption("--secondary-private-ip-addresses")]
    public string[]? SecondaryPrivateIpAddresses { get; set; }

    [CliOption("--secondary-private-ip-address-count")]
    public int? SecondaryPrivateIpAddressCount { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}