using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("networkmanager", "associate-customer-gateway")]
public record AwsNetworkmanagerAssociateCustomerGatewayOptions(
[property: CommandSwitch("--customer-gateway-arn")] string CustomerGatewayArn,
[property: CommandSwitch("--global-network-id")] string GlobalNetworkId,
[property: CommandSwitch("--device-id")] string DeviceId
) : AwsOptions
{
    [CommandSwitch("--link-id")]
    public string? LinkId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}