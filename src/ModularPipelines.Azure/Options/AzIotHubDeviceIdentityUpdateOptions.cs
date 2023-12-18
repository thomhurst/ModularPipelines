using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "hub", "device-identity", "update")]
public record AzIotHubDeviceIdentityUpdateOptions(
[property: CommandSwitch("--device-id")] string DeviceId
) : AzOptions
{
    [CommandSwitch("--add")]
    public string? Add { get; set; }

    [CommandSwitch("--am")]
    public string? Am { get; set; }

    [CommandSwitch("--auth-type")]
    public string? AuthType { get; set; }

    [BooleanCommandSwitch("--edge-enabled")]
    public bool? EdgeEnabled { get; set; }

    [CommandSwitch("--etag")]
    public string? Etag { get; set; }

    [BooleanCommandSwitch("--force-string")]
    public bool? ForceString { get; set; }

    [CommandSwitch("--hub-name")]
    public string? HubName { get; set; }

    [CommandSwitch("--login")]
    public string? Login { get; set; }

    [CommandSwitch("--pk")]
    public string? Pk { get; set; }

    [CommandSwitch("--primary-thumbprint")]
    public string? PrimaryThumbprint { get; set; }

    [CommandSwitch("--remove")]
    public string? Remove { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--secondary-key")]
    public string? SecondaryKey { get; set; }

    [CommandSwitch("--secondary-thumbprint")]
    public string? SecondaryThumbprint { get; set; }

    [CommandSwitch("--set")]
    public string? Set { get; set; }

    [CommandSwitch("--sta")]
    public string? Sta { get; set; }

    [CommandSwitch("--star")]
    public string? Star { get; set; }
}

