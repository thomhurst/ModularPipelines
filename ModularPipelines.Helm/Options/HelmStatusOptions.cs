using ModularPipelines.Attributes;

namespace ModularPipelines.Helm.Options;

[CommandPrecedingArguments("status")]
public record HelmStatusOptions : HelmOptions
{
    [CommandLongSwitch("output", SwitchValueSeparator = " ")]
    public string? Output { get; set; }

    [CommandLongSwitch("revision", SwitchValueSeparator = " ")]
    public int? Revision { get; set; }

    [BooleanCommandSwitch("show-desc")]
    public bool? ShowDesc { get; set; }

    [BooleanCommandSwitch("show-resources")]
    public bool? ShowResources { get; set; }

}
