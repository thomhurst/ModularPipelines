using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "hub", "device-identity", "export")]
public record AzIotHubDeviceIdentityExportOptions(
[property: CommandSwitch("--device-id")] string DeviceId,
[property: CommandSwitch("--hub-name")] string HubName,
[property: CommandSwitch("--key-type")] string KeyType
) : AzOptions
{
    [CommandSwitch("--auth-type")]
    public string? AuthType { get; set; }

    [CommandSwitch("--bc")]
    public string? Bc { get; set; }

    [CommandSwitch("--bcu")]
    public string? Bcu { get; set; }

    [CommandSwitch("--identity")]
    public string? Identity { get; set; }

    [BooleanCommandSwitch("--ik")]
    public bool? Ik { get; set; }

    [CommandSwitch("--login")]
    public string? Login { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--sa")]
    public string? Sa { get; set; }
}

