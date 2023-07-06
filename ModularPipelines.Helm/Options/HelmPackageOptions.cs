using ModularPipelines.Attributes;

namespace ModularPipelines.Helm.Options;

[CommandPrecedingArguments("package")]
public record HelmPackageOptions : HelmOptions
{
    [CommandLongSwitch("app-version", SwitchValueSeparator = " ")]
    public string? AppVersion { get; set; }

    [BooleanCommandSwitch("dependency-update")]
    public bool? DependencyUpdate { get; set; }

    [CommandLongSwitch("destination", SwitchValueSeparator = " ")]
    public string? Destination { get; set; }

    [CommandLongSwitch("key", SwitchValueSeparator = " ")]
    public string? Key { get; set; }

    [CommandLongSwitch("keyring", SwitchValueSeparator = " ")]
    public string? Keyring { get; set; }

    [CommandLongSwitch("passphrase-file", SwitchValueSeparator = " ")]
    public string? PassphraseFile { get; set; }

    [BooleanCommandSwitch("sign")]
    public bool? Sign { get; set; }

    [CommandLongSwitch("version", SwitchValueSeparator = " ")]
    public string? Version { get; set; }

}
