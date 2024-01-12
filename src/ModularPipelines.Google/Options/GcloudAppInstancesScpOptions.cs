using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("app", "instances", "scp")]
public record GcloudAppInstancesScpOptions(
[property: PositionalArgument] string Instance
) : GcloudOptions
{
    [BooleanCommandSwitch("--compress")]
    public bool? Compress { get; set; }

    [BooleanCommandSwitch("--recurse")]
    public bool? Recurse { get; set; }

    [CommandSwitch("--service")]
    public string? Service { get; set; }

    [BooleanCommandSwitch("--tunnel-through-iap")]
    public bool? TunnelThroughIap { get; set; }

    [CommandSwitch("--version")]
    public new string? Version { get; set; }
}