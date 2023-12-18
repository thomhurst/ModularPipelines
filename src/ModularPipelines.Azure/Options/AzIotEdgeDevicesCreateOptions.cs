using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "edge", "devices", "create")]
public record AzIotEdgeDevicesCreateOptions : AzOptions
{
    [CommandSwitch("--auth-type")]
    public string? AuthType { get; set; }

    [CommandSwitch("--cfg")]
    public string? Cfg { get; set; }

    [BooleanCommandSwitch("--clean")]
    public bool? Clean { get; set; }

    [CommandSwitch("--dct")]
    public string? Dct { get; set; }

    [CommandSwitch("--dea")]
    public string? Dea { get; set; }

    [CommandSwitch("--device")]
    public string? Device { get; set; }

    [CommandSwitch("--device-auth")]
    public string? DeviceAuth { get; set; }

    [CommandSwitch("--hub-name")]
    public string? HubName { get; set; }

    [CommandSwitch("--login")]
    public string? Login { get; set; }

    [CommandSwitch("--out")]
    public string? Out { get; set; }

    [CommandSwitch("--rc")]
    public string? Rc { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--rk")]
    public string? Rk { get; set; }

    [CommandSwitch("--root-pass")]
    public string? RootPass { get; set; }

    [BooleanCommandSwitch("--vis")]
    public bool? Vis { get; set; }

    [BooleanCommandSwitch("--yes")]
    public bool? Yes { get; set; }
}