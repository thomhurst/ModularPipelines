using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "enable-vgw-route-propagation")]
public record AwsEc2EnableVgwRoutePropagationOptions(
[property: CliOption("--gateway-id")] string GatewayId,
[property: CliOption("--route-table-id")] string RouteTableId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}