using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "modify-vpn-connection")]
public record AwsEc2ModifyVpnConnectionOptions(
[property: CommandSwitch("--vpn-connection-id")] string VpnConnectionId
) : AwsOptions
{
    [CommandSwitch("--transit-gateway-id")]
    public string? TransitGatewayId { get; set; }

    [CommandSwitch("--customer-gateway-id")]
    public string? CustomerGatewayId { get; set; }

    [CommandSwitch("--vpn-gateway-id")]
    public string? VpnGatewayId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}