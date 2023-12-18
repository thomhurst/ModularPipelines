using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "hub", "device-identity", "create")]
public record AzIotHubDeviceIdentityCreateOptions(
[property: CommandSwitch("--device-id")] string DeviceId
) : AzOptions
{
    [CommandSwitch("--am")]
    public string? Am { get; set; }

    [CommandSwitch("--auth-type")]
    public string? AuthType { get; set; }

    [CommandSwitch("--device-scope")]
    public string? DeviceScope { get; set; }

    [BooleanCommandSwitch("--edge-enabled")]
    public bool? EdgeEnabled { get; set; }

    [CommandSwitch("--hub-name")]
    public string? HubName { get; set; }

    [CommandSwitch("--login")]
    public string? Login { get; set; }

    [CommandSwitch("--od")]
    public string? Od { get; set; }

    [CommandSwitch("--pk")]
    public string? Pk { get; set; }

    [CommandSwitch("--primary-thumbprint")]
    public string? PrimaryThumbprint { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--secondary-key")]
    public string? SecondaryKey { get; set; }

    [CommandSwitch("--secondary-thumbprint")]
    public string? SecondaryThumbprint { get; set; }

    [CommandSwitch("--sta")]
    public string? Sta { get; set; }

    [CommandSwitch("--star")]
    public string? Star { get; set; }

    [CommandSwitch("--valid-days")]
    public string? ValidDays { get; set; }
}