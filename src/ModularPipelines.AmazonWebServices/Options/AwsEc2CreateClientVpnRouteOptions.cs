using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "create-client-vpn-route")]
public record AwsEc2CreateClientVpnRouteOptions(
[property: CliOption("--client-vpn-endpoint-id")] string ClientVpnEndpointId,
[property: CliOption("--destination-cidr-block")] string DestinationCidrBlock,
[property: CliOption("--target-vpc-subnet-id")] string TargetVpcSubnetId
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}