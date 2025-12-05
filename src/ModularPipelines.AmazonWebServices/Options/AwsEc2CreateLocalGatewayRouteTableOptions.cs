using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "create-local-gateway-route-table")]
public record AwsEc2CreateLocalGatewayRouteTableOptions(
[property: CliOption("--local-gateway-id")] string LocalGatewayId
) : AwsOptions
{
    [CliOption("--mode")]
    public string? Mode { get; set; }

    [CliOption("--tag-specifications")]
    public string[]? TagSpecifications { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}