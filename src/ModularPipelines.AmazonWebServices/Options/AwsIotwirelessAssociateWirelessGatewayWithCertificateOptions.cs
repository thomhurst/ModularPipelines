using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotwireless", "associate-wireless-gateway-with-certificate")]
public record AwsIotwirelessAssociateWirelessGatewayWithCertificateOptions(
[property: CliOption("--id")] string Id,
[property: CliOption("--iot-certificate-id")] string IotCertificateId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}