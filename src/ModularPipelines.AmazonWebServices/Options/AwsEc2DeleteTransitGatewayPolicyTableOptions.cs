using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "delete-transit-gateway-policy-table")]
public record AwsEc2DeleteTransitGatewayPolicyTableOptions(
[property: CommandSwitch("--transit-gateway-policy-table-id")] string TransitGatewayPolicyTableId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}