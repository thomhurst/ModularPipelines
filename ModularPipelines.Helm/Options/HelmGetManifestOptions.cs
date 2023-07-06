using ModularPipelines.Attributes;

namespace ModularPipelines.Helm.Options;

[CommandPrecedingArguments("get", "manifest")]
public record HelmGetManifestOptions : HelmOptions
{
    [CommandLongSwitch("revision", SwitchValueSeparator = " ")]
    public int? Revision { get; set; }

}
