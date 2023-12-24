using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "delete-local-gateway-route")]
public record AwsEc2DeleteLocalGatewayRouteOptions(
[property: CommandSwitch("--local-gateway-route-table-id")] string LocalGatewayRouteTableId
) : AwsOptions
{
    [CommandSwitch("--destination-cidr-block")]
    public string? DestinationCidrBlock { get; set; }

    [CommandSwitch("--destination-prefix-list-id")]
    public string? DestinationPrefixListId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}