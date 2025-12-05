using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "delete-local-gateway-route")]
public record AwsEc2DeleteLocalGatewayRouteOptions(
[property: CliOption("--local-gateway-route-table-id")] string LocalGatewayRouteTableId
) : AwsOptions
{
    [CliOption("--destination-cidr-block")]
    public string? DestinationCidrBlock { get; set; }

    [CliOption("--destination-prefix-list-id")]
    public string? DestinationPrefixListId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}