using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sphere", "device", "wifi", "add")]
public record AzSphereDeviceWifiAddOptions(
[property: CliOption("--ssid")] string Ssid
) : AzOptions
{
    [CliOption("--client-cert-id")]
    public string? ClientCertId { get; set; }

    [CliOption("--client-id")]
    public string? ClientId { get; set; }

    [CliOption("--config-name")]
    public string? ConfigName { get; set; }

    [CliOption("--device")]
    public string? Device { get; set; }

    [CliOption("--psk")]
    public string? Psk { get; set; }

    [CliOption("--root-ca-cert-id")]
    public string? RootCaCertId { get; set; }

    [CliFlag("--targeted-scan")]
    public bool? TargetedScan { get; set; }
}