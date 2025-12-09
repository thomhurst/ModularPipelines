using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "modify-vpn-connection")]
public record AwsEc2ModifyVpnConnectionOptions(
[property: CliOption("--vpn-connection-id")] string VpnConnectionId
) : AwsOptions
{
    [CliOption("--transit-gateway-id")]
    public string? TransitGatewayId { get; set; }

    [CliOption("--customer-gateway-id")]
    public string? CustomerGatewayId { get; set; }

    [CliOption("--vpn-gateway-id")]
    public string? VpnGatewayId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}