using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "export-transit-gateway-routes")]
public record AwsEc2ExportTransitGatewayRoutesOptions(
[property: CliOption("--transit-gateway-route-table-id")] string TransitGatewayRouteTableId,
[property: CliOption("--s3-bucket")] string S3Bucket
) : AwsOptions
{
    [CliOption("--filters")]
    public string[]? Filters { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}