using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("networkmanager", "disassociate-customer-gateway")]
public record AwsNetworkmanagerDisassociateCustomerGatewayOptions(
[property: CliOption("--global-network-id")] string GlobalNetworkId,
[property: CliOption("--customer-gateway-arn")] string CustomerGatewayArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}