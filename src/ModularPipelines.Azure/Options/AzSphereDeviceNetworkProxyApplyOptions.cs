using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sphere", "device", "network", "proxy", "apply")]
public record AzSphereDeviceNetworkProxyApplyOptions(
[property: CommandSwitch("--address")] string Address,
[property: CommandSwitch("--authentication")] string Authentication,
[property: CommandSwitch("--port")] int Port
) : AzOptions
{
    [CommandSwitch("--device")]
    public string? Device { get; set; }

    [BooleanCommandSwitch("--disable")]
    public bool? Disable { get; set; }

    [BooleanCommandSwitch("--enable")]
    public bool? Enable { get; set; }

    [BooleanCommandSwitch("--no-proxy-addresses")]
    public bool? NoProxyAddresses { get; set; }

    [CommandSwitch("--password")]
    public string? Password { get; set; }

    [CommandSwitch("--username")]
    public string? Username { get; set; }
}