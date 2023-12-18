using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "device", "c2d-message", "receive")]
public record AzIotDeviceC2dMessageReceiveOptions(
[property: CommandSwitch("--device-id")] string DeviceId
) : AzOptions
{
    [BooleanCommandSwitch("--abandon")]
    public bool? Abandon { get; set; }

    [BooleanCommandSwitch("--complete")]
    public bool? Complete { get; set; }

    [CommandSwitch("--hub-name")]
    public string? HubName { get; set; }

    [CommandSwitch("--lock-timeout")]
    public string? LockTimeout { get; set; }

    [CommandSwitch("--login")]
    public string? Login { get; set; }

    [BooleanCommandSwitch("--reject")]
    public bool? Reject { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}