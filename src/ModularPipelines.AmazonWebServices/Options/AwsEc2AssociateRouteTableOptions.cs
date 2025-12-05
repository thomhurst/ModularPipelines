using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "associate-route-table")]
public record AwsEc2AssociateRouteTableOptions(
[property: CliOption("--route-table-id")] string RouteTableId
) : AwsOptions
{
    [CliOption("--subnet-id")]
    public string? SubnetId { get; set; }

    [CliOption("--gateway-id")]
    public string? GatewayId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}