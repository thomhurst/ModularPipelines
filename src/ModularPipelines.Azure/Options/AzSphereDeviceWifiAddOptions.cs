using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sphere", "device", "wifi", "add")]
public record AzSphereDeviceWifiAddOptions(
[property: CommandSwitch("--ssid")] string Ssid
) : AzOptions
{
    [CommandSwitch("--client-cert-id")]
    public string? ClientCertId { get; set; }

    [CommandSwitch("--client-id")]
    public string? ClientId { get; set; }

    [CommandSwitch("--config-name")]
    public string? ConfigName { get; set; }

    [CommandSwitch("--device")]
    public string? Device { get; set; }

    [CommandSwitch("--psk")]
    public string? Psk { get; set; }

    [CommandSwitch("--root-ca-cert-id")]
    public string? RootCaCertId { get; set; }

    [BooleanCommandSwitch("--targeted-scan")]
    public bool? TargetedScan { get; set; }
}