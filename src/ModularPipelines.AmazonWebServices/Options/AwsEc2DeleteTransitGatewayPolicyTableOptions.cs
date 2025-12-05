using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "delete-transit-gateway-policy-table")]
public record AwsEc2DeleteTransitGatewayPolicyTableOptions(
[property: CliOption("--transit-gateway-policy-table-id")] string TransitGatewayPolicyTableId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}