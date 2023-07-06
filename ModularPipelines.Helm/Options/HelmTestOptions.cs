using ModularPipelines.Attributes;

namespace ModularPipelines.Helm.Options;

[CommandPrecedingArguments("test")]
public record HelmTestOptions : HelmOptions
{
    [CommandLongSwitch("filter", SwitchValueSeparator = " ")]
    public string[]? Filter { get; set; }

    [BooleanCommandSwitch("logs")]
    public bool? Logs { get; set; }

    [CommandLongSwitch("timeout", SwitchValueSeparator = " ")]
    public string? Timeout { get; set; }

}
