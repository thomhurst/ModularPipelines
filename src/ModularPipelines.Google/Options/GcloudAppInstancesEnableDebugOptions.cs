using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("app", "instances", "enable-debug")]
public record GcloudAppInstancesEnableDebugOptions(
[property: PositionalArgument] string Instance
) : GcloudOptions
{
    [CommandSwitch("--service")]
    public string? Service { get; set; }

    [CommandSwitch("--version")]
    public new string? Version { get; set; }
}