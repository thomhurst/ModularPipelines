using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Helm.Options;

[CommandPrecedingArguments("lint")]
[ExcludeFromCodeCoverage]
public record HelmLintOptions : HelmOptions
{
    [BooleanCommandSwitch("--quiet")]
    public virtual bool? Quiet { get; set; }

    [CommandEqualsSeparatorSwitch("--set", SwitchValueSeparator = " ")]
    public string[]? Set { get; set; }

    [CommandEqualsSeparatorSwitch("--set-file", SwitchValueSeparator = " ")]
    public string[]? SetFile { get; set; }

    [CommandEqualsSeparatorSwitch("--set-json", SwitchValueSeparator = " ")]
    public string[]? SetJson { get; set; }

    [CommandEqualsSeparatorSwitch("--set-literal", SwitchValueSeparator = " ")]
    public string[]? SetLiteral { get; set; }

    [CommandEqualsSeparatorSwitch("--set-string", SwitchValueSeparator = " ")]
    public string[]? SetString { get; set; }

    [BooleanCommandSwitch("--strict")]
    public virtual bool? Strict { get; set; }

    [CommandEqualsSeparatorSwitch("--values", SwitchValueSeparator = " ")]
    public string[]? Values { get; set; }

    [BooleanCommandSwitch("--with-subcharts")]
    public virtual bool? WithSubcharts { get; set; }
}