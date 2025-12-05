using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("networkmanager", "associate-customer-gateway")]
public record AwsNetworkmanagerAssociateCustomerGatewayOptions(
[property: CliOption("--customer-gateway-arn")] string CustomerGatewayArn,
[property: CliOption("--global-network-id")] string GlobalNetworkId,
[property: CliOption("--device-id")] string DeviceId
) : AwsOptions
{
    [CliOption("--link-id")]
    public string? LinkId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}