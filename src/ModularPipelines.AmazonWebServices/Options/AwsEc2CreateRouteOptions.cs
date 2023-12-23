using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "create-route")]
public record AwsEc2CreateRouteOptions(
[property: CommandSwitch("--route-table-id")] string RouteTableId
) : AwsOptions
{
    [CommandSwitch("--destination-cidr-block")]
    public string? DestinationCidrBlock { get; set; }

    [CommandSwitch("--destination-ipv6-cidr-block")]
    public string? DestinationIpv6CidrBlock { get; set; }

    [CommandSwitch("--destination-prefix-list-id")]
    public string? DestinationPrefixListId { get; set; }

    [CommandSwitch("--vpc-endpoint-id")]
    public string? VpcEndpointId { get; set; }

    [CommandSwitch("--egress-only-internet-gateway-id")]
    public string? EgressOnlyInternetGatewayId { get; set; }

    [CommandSwitch("--gateway-id")]
    public string? GatewayId { get; set; }

    [CommandSwitch("--instance-id")]
    public string? InstanceId { get; set; }

    [CommandSwitch("--nat-gateway-id")]
    public string? NatGatewayId { get; set; }

    [CommandSwitch("--transit-gateway-id")]
    public string? TransitGatewayId { get; set; }

    [CommandSwitch("--local-gateway-id")]
    public string? LocalGatewayId { get; set; }

    [CommandSwitch("--carrier-gateway-id")]
    public string? CarrierGatewayId { get; set; }

    [CommandSwitch("--network-interface-id")]
    public string? NetworkInterfaceId { get; set; }

    [CommandSwitch("--vpc-peering-connection-id")]
    public string? VpcPeeringConnectionId { get; set; }

    [CommandSwitch("--core-network-arn")]
    public string? CoreNetworkArn { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}