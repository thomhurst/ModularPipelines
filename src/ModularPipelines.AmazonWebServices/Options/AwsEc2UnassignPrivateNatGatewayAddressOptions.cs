using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "unassign-private-nat-gateway-address")]
public record AwsEc2UnassignPrivateNatGatewayAddressOptions(
[property: CliOption("--nat-gateway-id")] string NatGatewayId,
[property: CliOption("--private-ip-addresses")] string[] PrivateIpAddresses
) : AwsOptions
{
    [CliOption("--max-drain-duration-seconds")]
    public int? MaxDrainDurationSeconds { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}