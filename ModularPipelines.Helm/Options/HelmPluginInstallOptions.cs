using ModularPipelines.Attributes;

namespace ModularPipelines.Helm.Options;

[CommandPrecedingArguments("plugin", "install")]
public record HelmPluginInstallOptions : HelmOptions
{
    [CommandLongSwitch("version", SwitchValueSeparator = " ")]
    public string? Version { get; set; }

}
