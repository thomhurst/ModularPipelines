using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "delete-local-gateway-route-table")]
public record AwsEc2DeleteLocalGatewayRouteTableOptions(
[property: CliOption("--local-gateway-route-table-id")] string LocalGatewayRouteTableId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}