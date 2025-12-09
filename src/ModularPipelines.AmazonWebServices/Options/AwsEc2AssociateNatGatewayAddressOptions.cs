using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "associate-nat-gateway-address")]
public record AwsEc2AssociateNatGatewayAddressOptions(
[property: CliOption("--nat-gateway-id")] string NatGatewayId,
[property: CliOption("--allocation-ids")] string[] AllocationIds
) : AwsOptions
{
    [CliOption("--private-ip-addresses")]
    public string[]? PrivateIpAddresses { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}