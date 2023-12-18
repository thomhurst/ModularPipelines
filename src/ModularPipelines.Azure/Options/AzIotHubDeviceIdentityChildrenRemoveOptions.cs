using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "hub", "device-identity", "children", "remove")]
public record AzIotHubDeviceIdentityChildrenRemoveOptions(
[property: CommandSwitch("--device-id")] string DeviceId
) : AzOptions
{
    [CommandSwitch("--auth-type")]
    public string? AuthType { get; set; }

    [CommandSwitch("--child-list")]
    public string? ChildList { get; set; }

    [CommandSwitch("--hub-name")]
    public string? HubName { get; set; }

    [CommandSwitch("--login")]
    public string? Login { get; set; }

    [BooleanCommandSwitch("--remove-all")]
    public bool? RemoveAll { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}