using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "vpn-connection", "show-device-config-script")]
public record AzNetworkVpnConnectionShowDeviceConfigScriptOptions(
[property: CliOption("--device-family")] string DeviceFamily,
[property: CliOption("--firmware-version")] string FirmwareVersion,
[property: CliOption("--vendor")] string Vendor
) : AzOptions
{
    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}