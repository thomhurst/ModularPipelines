using ModularPipelines.Attributes;

namespace ModularPipelines.Helm.Options;

[CommandPrecedingArguments("lint")]
public record HelmLintOptions : HelmOptions
{
    [BooleanCommandSwitch("quiet")]
    public bool? Quiet { get; set; }

    [CommandLongSwitch("set", SwitchValueSeparator = " ")]
    public string[]? Set { get; set; }

    [CommandLongSwitch("set-file", SwitchValueSeparator = " ")]
    public string[]? SetFile { get; set; }

    [CommandLongSwitch("set-json", SwitchValueSeparator = " ")]
    public string[]? SetJson { get; set; }

    [CommandLongSwitch("set-literal", SwitchValueSeparator = " ")]
    public string[]? SetLiteral { get; set; }

    [CommandLongSwitch("set-string", SwitchValueSeparator = " ")]
    public string[]? SetString { get; set; }

    [BooleanCommandSwitch("strict")]
    public bool? Strict { get; set; }

    [CommandLongSwitch("values", SwitchValueSeparator = " ")]
    public string[]? Values { get; set; }

    [BooleanCommandSwitch("with-subcharts")]
    public bool? WithSubcharts { get; set; }

}
