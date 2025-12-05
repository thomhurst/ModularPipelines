using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "disassociate-transit-gateway-policy-table")]
public record AwsEc2DisassociateTransitGatewayPolicyTableOptions(
[property: CliOption("--transit-gateway-policy-table-id")] string TransitGatewayPolicyTableId,
[property: CliOption("--transit-gateway-attachment-id")] string TransitGatewayAttachmentId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}