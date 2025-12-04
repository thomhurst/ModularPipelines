using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("iot", "edge", "devices", "create")]
public record AzIotEdgeDevicesCreateOptions : AzOptions
{
    [CliOption("--auth-type")]
    public string? AuthType { get; set; }

    [CliOption("--cfg")]
    public string? Cfg { get; set; }

    [CliFlag("--clean")]
    public bool? Clean { get; set; }

    [CliOption("--dct")]
    public string? Dct { get; set; }

    [CliOption("--dea")]
    public string? Dea { get; set; }

    [CliOption("--device")]
    public string? Device { get; set; }

    [CliOption("--device-auth")]
    public string? DeviceAuth { get; set; }

    [CliOption("--hub-name")]
    public string? HubName { get; set; }

    [CliOption("--login")]
    public string? Login { get; set; }

    [CliOption("--out")]
    public string? Out { get; set; }

    [CliOption("--rc")]
    public string? Rc { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--rk")]
    public string? Rk { get; set; }

    [CliOption("--root-pass")]
    public string? RootPass { get; set; }

    [CliFlag("--vis")]
    public bool? Vis { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}