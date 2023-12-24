using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "disassociate-nat-gateway-address")]
public record AwsEc2DisassociateNatGatewayAddressOptions(
[property: CommandSwitch("--nat-gateway-id")] string NatGatewayId,
[property: CommandSwitch("--association-ids")] string[] AssociationIds
) : AwsOptions
{
    [CommandSwitch("--max-drain-duration-seconds")]
    public int? MaxDrainDurationSeconds { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}