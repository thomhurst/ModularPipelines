using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "create-vpn-connection")]
public record AwsEc2CreateVpnConnectionOptions(
[property: CliOption("--customer-gateway-id")] string CustomerGatewayId,
[property: CliOption("--type")] string Type
) : AwsOptions
{
    [CliOption("--vpn-gateway-id")]
    public string? VpnGatewayId { get; set; }

    [CliOption("--transit-gateway-id")]
    public string? TransitGatewayId { get; set; }

    [CliOption("--options")]
    public string? Options { get; set; }

    [CliOption("--tag-specifications")]
    public string[]? TagSpecifications { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}