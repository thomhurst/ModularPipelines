using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "vpn-connection", "show-device-config-script")]
public record AzNetworkVpnConnectionShowDeviceConfigScriptOptions(
[property: CommandSwitch("--device-family")] string DeviceFamily,
[property: CommandSwitch("--firmware-version")] string FirmwareVersion,
[property: CommandSwitch("--vendor")] string Vendor
) : AzOptions
{
    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--subscription")]
    public new string? Subscription { get; set; }
}