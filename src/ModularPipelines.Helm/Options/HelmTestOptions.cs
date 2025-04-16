using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Helm.Options;

[CommandPrecedingArguments("test")]
[ExcludeFromCodeCoverage]
public record HelmTestOptions : HelmOptions
{
    [CommandEqualsSeparatorSwitch("--filter", SwitchValueSeparator = " ")]
    public string[]? Filter { get; set; }

    [BooleanCommandSwitch("--logs")]
    public virtual bool? Logs { get; set; }

    [CommandEqualsSeparatorSwitch("--timeout", SwitchValueSeparator = " ")]
    public string? Timeout { get; set; }
}