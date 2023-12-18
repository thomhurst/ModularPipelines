using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "hub", "device-identity", "delete")]
public record AzIotHubDeviceIdentityDeleteOptions(
[property: CommandSwitch("--device-id")] string DeviceId
) : AzOptions
{
    [CommandSwitch("--auth-type")]
    public string? AuthType { get; set; }

    [CommandSwitch("--etag")]
    public string? Etag { get; set; }

    [CommandSwitch("--hub-name")]
    public string? HubName { get; set; }

    [CommandSwitch("--login")]
    public string? Login { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}