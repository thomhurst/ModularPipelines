using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "get-vpn-connection-device-sample-configuration")]
public record AwsEc2GetVpnConnectionDeviceSampleConfigurationOptions(
[property: CliOption("--vpn-connection-id")] string VpnConnectionId,
[property: CliOption("--vpn-connection-device-type-id")] string VpnConnectionDeviceTypeId
) : AwsOptions
{
    [CliOption("--internet-key-exchange-version")]
    public string? InternetKeyExchangeVersion { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}