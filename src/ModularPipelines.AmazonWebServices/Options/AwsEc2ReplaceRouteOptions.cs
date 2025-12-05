using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "replace-route")]
public record AwsEc2ReplaceRouteOptions(
[property: CliOption("--route-table-id")] string RouteTableId
) : AwsOptions
{
    [CliOption("--destination-cidr-block")]
    public string? DestinationCidrBlock { get; set; }

    [CliOption("--destination-ipv6-cidr-block")]
    public string? DestinationIpv6CidrBlock { get; set; }

    [CliOption("--destination-prefix-list-id")]
    public string? DestinationPrefixListId { get; set; }

    [CliOption("--vpc-endpoint-id")]
    public string? VpcEndpointId { get; set; }

    [CliOption("--egress-only-internet-gateway-id")]
    public string? EgressOnlyInternetGatewayId { get; set; }

    [CliOption("--gateway-id")]
    public string? GatewayId { get; set; }

    [CliOption("--instance-id")]
    public string? InstanceId { get; set; }

    [CliOption("--nat-gateway-id")]
    public string? NatGatewayId { get; set; }

    [CliOption("--transit-gateway-id")]
    public string? TransitGatewayId { get; set; }

    [CliOption("--local-gateway-id")]
    public string? LocalGatewayId { get; set; }

    [CliOption("--carrier-gateway-id")]
    public string? CarrierGatewayId { get; set; }

    [CliOption("--network-interface-id")]
    public string? NetworkInterfaceId { get; set; }

    [CliOption("--vpc-peering-connection-id")]
    public string? VpcPeeringConnectionId { get; set; }

    [CliOption("--core-network-arn")]
    public string? CoreNetworkArn { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}