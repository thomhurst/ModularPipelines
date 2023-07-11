using ModularPipelines.Attributes;

namespace ModularPipelines.Helm.Options;

[CommandPrecedingArguments("list")]
public record HelmListOptions : HelmOptions
{
    [BooleanCommandSwitch("--all")]
    public bool? All { get; set; }

    [BooleanCommandSwitch("--all-namespaces")]
    public bool? AllNamespaces { get; set; }

    [BooleanCommandSwitch("--date")]
    public bool? Date { get; set; }

    [BooleanCommandSwitch("--deployed")]
    public bool? Deployed { get; set; }

    [BooleanCommandSwitch("--failed")]
    public bool? Failed { get; set; }

    [CommandEqualsSeparatorSwitch("--filter", SwitchValueSeparator = " ")]
    public string? Filter { get; set; }

    [CommandEqualsSeparatorSwitch("--max", SwitchValueSeparator = " ")]
    public int? Max { get; set; }

    [BooleanCommandSwitch("--no-headers")]
    public bool? NoHeaders { get; set; }

    [CommandEqualsSeparatorSwitch("--offset", SwitchValueSeparator = " ")]
    public int? Offset { get; set; }

    [CommandEqualsSeparatorSwitch("--output", SwitchValueSeparator = " ")]
    public string? Output { get; set; }

    [BooleanCommandSwitch("--pending")]
    public bool? Pending { get; set; }

    [BooleanCommandSwitch("--reverse")]
    public bool? Reverse { get; set; }

    [CommandEqualsSeparatorSwitch("--selector", SwitchValueSeparator = " ")]
    public string? Selector { get; set; }

    [BooleanCommandSwitch("--short")]
    public bool? Short { get; set; }

    [BooleanCommandSwitch("--superseded")]
    public bool? Superseded { get; set; }

    [CommandEqualsSeparatorSwitch("--time-format", SwitchValueSeparator = " ")]
    public string? TimeFormat { get; set; }

    [BooleanCommandSwitch("--uninstalled")]
    public bool? Uninstalled { get; set; }

    [BooleanCommandSwitch("--uninstalling")]
    public bool? Uninstalling { get; set; }
}
