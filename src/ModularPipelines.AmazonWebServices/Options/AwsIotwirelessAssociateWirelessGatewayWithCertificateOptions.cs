using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iotwireless", "associate-wireless-gateway-with-certificate")]
public record AwsIotwirelessAssociateWirelessGatewayWithCertificateOptions(
[property: CommandSwitch("--id")] string Id,
[property: CommandSwitch("--iot-certificate-id")] string IotCertificateId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}