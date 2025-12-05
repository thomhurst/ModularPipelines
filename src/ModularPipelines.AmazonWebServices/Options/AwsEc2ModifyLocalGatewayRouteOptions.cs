using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "modify-local-gateway-route")]
public record AwsEc2ModifyLocalGatewayRouteOptions(
[property: CliOption("--local-gateway-route-table-id")] string LocalGatewayRouteTableId
) : AwsOptions
{
    [CliOption("--destination-cidr-block")]
    public string? DestinationCidrBlock { get; set; }

    [CliOption("--local-gateway-virtual-interface-group-id")]
    public string? LocalGatewayVirtualInterfaceGroupId { get; set; }

    [CliOption("--network-interface-id")]
    public string? NetworkInterfaceId { get; set; }

    [CliOption("--destination-prefix-list-id")]
    public string? DestinationPrefixListId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}