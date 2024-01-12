using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("app", "instances", "ssh")]
public record GcloudAppInstancesSshOptions(
[property: PositionalArgument] string Instance,
[property: PositionalArgument] string Command
) : GcloudOptions
{
    [CommandSwitch("--container")]
    public string? Container { get; set; }

    [CommandSwitch("--service")]
    public string? Service { get; set; }

    [BooleanCommandSwitch("--tunnel-through-iap")]
    public bool? TunnelThroughIap { get; set; }

    [CommandSwitch("--version")]
    public new string? Version { get; set; }
}