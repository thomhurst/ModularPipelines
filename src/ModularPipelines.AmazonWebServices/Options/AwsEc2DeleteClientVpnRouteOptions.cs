using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "delete-client-vpn-route")]
public record AwsEc2DeleteClientVpnRouteOptions(
[property: CliOption("--client-vpn-endpoint-id")] string ClientVpnEndpointId,
[property: CliOption("--destination-cidr-block")] string DestinationCidrBlock
) : AwsOptions
{
    [CliOption("--target-vpc-subnet-id")]
    public string? TargetVpcSubnetId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}