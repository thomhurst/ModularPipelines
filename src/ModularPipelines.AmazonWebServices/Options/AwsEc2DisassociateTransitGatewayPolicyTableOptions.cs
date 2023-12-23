using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "disassociate-transit-gateway-policy-table")]
public record AwsEc2DisassociateTransitGatewayPolicyTableOptions(
[property: CommandSwitch("--transit-gateway-policy-table-id")] string TransitGatewayPolicyTableId,
[property: CommandSwitch("--transit-gateway-attachment-id")] string TransitGatewayAttachmentId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}