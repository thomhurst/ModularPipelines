using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sphere", "device", "network", "proxy", "apply")]
public record AzSphereDeviceNetworkProxyApplyOptions(
[property: CliOption("--address")] string Address,
[property: CliOption("--authentication")] string Authentication,
[property: CliOption("--port")] int Port
) : AzOptions
{
    [CliOption("--device")]
    public string? Device { get; set; }

    [CliFlag("--disable")]
    public bool? Disable { get; set; }

    [CliFlag("--enable")]
    public bool? Enable { get; set; }

    [CliFlag("--no-proxy-addresses")]
    public bool? NoProxyAddresses { get; set; }

    [CliOption("--password")]
    public string? Password { get; set; }

    [CliOption("--username")]
    public string? Username { get; set; }
}