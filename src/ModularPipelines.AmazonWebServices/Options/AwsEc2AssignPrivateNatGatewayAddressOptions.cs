using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "assign-private-nat-gateway-address")]
public record AwsEc2AssignPrivateNatGatewayAddressOptions(
[property: CliOption("--nat-gateway-id")] string NatGatewayId
) : AwsOptions
{
    [CliOption("--private-ip-addresses")]
    public string[]? PrivateIpAddresses { get; set; }

    [CliOption("--private-ip-address-count")]
    public int? PrivateIpAddressCount { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}