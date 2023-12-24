using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "create-vpn-connection")]
public record AwsEc2CreateVpnConnectionOptions(
[property: CommandSwitch("--customer-gateway-id")] string CustomerGatewayId,
[property: CommandSwitch("--type")] string Type
) : AwsOptions
{
    [CommandSwitch("--vpn-gateway-id")]
    public string? VpnGatewayId { get; set; }

    [CommandSwitch("--transit-gateway-id")]
    public string? TransitGatewayId { get; set; }

    [CommandSwitch("--options")]
    public string? Options { get; set; }

    [CommandSwitch("--tag-specifications")]
    public string[]? TagSpecifications { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}