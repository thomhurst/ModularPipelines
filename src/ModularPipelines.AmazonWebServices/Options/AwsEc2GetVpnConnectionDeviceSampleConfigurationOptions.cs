using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "get-vpn-connection-device-sample-configuration")]
public record AwsEc2GetVpnConnectionDeviceSampleConfigurationOptions(
[property: CommandSwitch("--vpn-connection-id")] string VpnConnectionId,
[property: CommandSwitch("--vpn-connection-device-type-id")] string VpnConnectionDeviceTypeId
) : AwsOptions
{
    [CommandSwitch("--internet-key-exchange-version")]
    public string? InternetKeyExchangeVersion { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}