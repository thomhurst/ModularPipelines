using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "create-transit-gateway-policy-table")]
public record AwsEc2CreateTransitGatewayPolicyTableOptions(
[property: CliOption("--transit-gateway-id")] string TransitGatewayId
) : AwsOptions
{
    [CliOption("--tag-specifications")]
    public string[]? TagSpecifications { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}