using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Helm.Options;

[CommandPrecedingArguments("list")]
[ExcludeFromCodeCoverage]
public record HelmListOptions : HelmOptions
{
    [BooleanCommandSwitch("--all")]
    public virtual bool? All { get; set; }

    [BooleanCommandSwitch("--all-namespaces")]
    public virtual bool? AllNamespaces { get; set; }

    [BooleanCommandSwitch("--date")]
    public virtual bool? Date { get; set; }

    [BooleanCommandSwitch("--deployed")]
    public virtual bool? Deployed { get; set; }

    [BooleanCommandSwitch("--failed")]
    public virtual bool? Failed { get; set; }

    [CommandEqualsSeparatorSwitch("--filter", SwitchValueSeparator = " ")]
    public string? Filter { get; set; }

    [CommandEqualsSeparatorSwitch("--max", SwitchValueSeparator = " ")]
    public int? Max { get; set; }

    [BooleanCommandSwitch("--no-headers")]
    public virtual bool? NoHeaders { get; set; }

    [CommandEqualsSeparatorSwitch("--offset", SwitchValueSeparator = " ")]
    public int? Offset { get; set; }

    [CommandEqualsSeparatorSwitch("--output", SwitchValueSeparator = " ")]
    public string? Output { get; set; }

    [BooleanCommandSwitch("--pending")]
    public virtual bool? Pending { get; set; }

    [BooleanCommandSwitch("--reverse")]
    public virtual bool? Reverse { get; set; }

    [CommandEqualsSeparatorSwitch("--selector", SwitchValueSeparator = " ")]
    public string? Selector { get; set; }

    [BooleanCommandSwitch("--short")]
    public virtual bool? Short { get; set; }

    [BooleanCommandSwitch("--superseded")]
    public virtual bool? Superseded { get; set; }

    [CommandEqualsSeparatorSwitch("--time-format", SwitchValueSeparator = " ")]
    public string? TimeFormat { get; set; }

    [BooleanCommandSwitch("--uninstalled")]
    public virtual bool? Uninstalled { get; set; }

    [BooleanCommandSwitch("--uninstalling")]
    public virtual bool? Uninstalling { get; set; }
}