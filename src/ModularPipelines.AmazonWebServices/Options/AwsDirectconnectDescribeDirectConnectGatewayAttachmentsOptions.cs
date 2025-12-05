using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("directconnect", "describe-direct-connect-gateway-attachments")]
public record AwsDirectconnectDescribeDirectConnectGatewayAttachmentsOptions : AwsOptions
{
    [CliOption("--direct-connect-gateway-id")]
    public string? DirectConnectGatewayId { get; set; }

    [CliOption("--virtual-interface-id")]
    public string? VirtualInterfaceId { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}