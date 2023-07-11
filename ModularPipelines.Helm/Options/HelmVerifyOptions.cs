using ModularPipelines.Attributes;

namespace ModularPipelines.Helm.Options;

[CommandPrecedingArguments("verify")]
public record HelmVerifyOptions : HelmOptions
{
    [CommandEqualsSeparatorSwitch("--keyring", SwitchValueSeparator = " ")]
    public string? Keyring { get; set; }
}
