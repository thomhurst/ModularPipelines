using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("directconnect", "describe-direct-connect-gateway-attachments")]
public record AwsDirectconnectDescribeDirectConnectGatewayAttachmentsOptions : AwsOptions
{
    [CommandSwitch("--direct-connect-gateway-id")]
    public string? DirectConnectGatewayId { get; set; }

    [CommandSwitch("--virtual-interface-id")]
    public string? VirtualInterfaceId { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}