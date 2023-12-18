using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "hub", "device-twin", "list")]
public record AzIotHubDeviceTwinListOptions : AzOptions
{
    [CommandSwitch("--auth-type")]
    public string? AuthType { get; set; }

    [BooleanCommandSwitch("--edge-enabled")]
    public bool? EdgeEnabled { get; set; }

    [CommandSwitch("--hub-name")]
    public string? HubName { get; set; }

    [CommandSwitch("--login")]
    public string? Login { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--top")]
    public string? Top { get; set; }
}