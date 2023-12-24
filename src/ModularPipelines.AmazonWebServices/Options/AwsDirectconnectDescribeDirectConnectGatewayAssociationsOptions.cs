using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("directconnect", "describe-direct-connect-gateway-associations")]
public record AwsDirectconnectDescribeDirectConnectGatewayAssociationsOptions : AwsOptions
{
    [CommandSwitch("--association-id")]
    public string? AssociationId { get; set; }

    [CommandSwitch("--associated-gateway-id")]
    public string? AssociatedGatewayId { get; set; }

    [CommandSwitch("--direct-connect-gateway-id")]
    public string? DirectConnectGatewayId { get; set; }

    [CommandSwitch("--virtual-gateway-id")]
    public string? VirtualGatewayId { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}