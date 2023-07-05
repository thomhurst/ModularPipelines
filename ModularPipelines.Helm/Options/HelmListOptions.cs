using ModularPipelines.Attributes;

namespace ModularPipelines.Helm.Options;

public record HelmListOptions : HelmOptions
{
    [BooleanCommandSwitch("all")]
    public bool? All { get; set; }

    [CommandLongSwitch("all-namespaces", SwitchValueSeparator = " ")]
    public string? AllNamespaces { get; set; }

    [BooleanCommandSwitch("date")]
    public bool? Date { get; set; }

    [BooleanCommandSwitch("deployed")]
    public bool? Deployed { get; set; }

    [BooleanCommandSwitch("failed")]
    public bool? Failed { get; set; }

    [CommandLongSwitch("filter", SwitchValueSeparator = " ")]
    public string? Filter { get; set; }

    [BooleanCommandSwitch("help")]
    public bool? Help { get; set; }

    [CommandLongSwitch("max", SwitchValueSeparator = " ")]
    public int? Max { get; set; }

    [BooleanCommandSwitch("no-headers")]
    public bool? NoHeaders { get; set; }

    [CommandLongSwitch("offset", SwitchValueSeparator = " ")]
    public int? Offset { get; set; }

    [CommandLongSwitch("output", SwitchValueSeparator = " ")]
    public string? Output { get; set; }

    [BooleanCommandSwitch("pending")]
    public bool? Pending { get; set; }

    [BooleanCommandSwitch("reverse")]
    public bool? Reverse { get; set; }

    [CommandLongSwitch("selector", SwitchValueSeparator = " ")]
    public string? Selector { get; set; }

    [BooleanCommandSwitch("short")]
    public bool? Short { get; set; }

    [BooleanCommandSwitch("superseded")]
    public bool? Superseded { get; set; }

    [CommandLongSwitch("time-format", SwitchValueSeparator = " ")]
    public string? TimeFormat { get; set; }

    [BooleanCommandSwitch("uninstalled")]
    public bool? Uninstalled { get; set; }

    [BooleanCommandSwitch("uninstalling")]
    public bool? Uninstalling { get; set; }


}