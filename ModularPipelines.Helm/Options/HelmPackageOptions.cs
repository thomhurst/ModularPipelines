using ModularPipelines.Attributes;

namespace ModularPipelines.Helm.Options;

[CommandPrecedingArguments("package")]
public record HelmPackageOptions : HelmOptions
{
    [CommandEqualsSeparatorSwitch("--app-version", SwitchValueSeparator = " ")]
    public string? AppVersion { get; set; }

    [BooleanCommandSwitch("--dependency-update")]
    public bool? DependencyUpdate { get; set; }

    [CommandEqualsSeparatorSwitch("--destination", SwitchValueSeparator = " ")]
    public string? Destination { get; set; }

    [CommandEqualsSeparatorSwitch("--key", SwitchValueSeparator = " ")]
    public string? Key { get; set; }

    [CommandEqualsSeparatorSwitch("--keyring", SwitchValueSeparator = " ")]
    public string? Keyring { get; set; }

    [CommandEqualsSeparatorSwitch("--passphrase-file", SwitchValueSeparator = " ")]
    public string? PassphraseFile { get; set; }

    [BooleanCommandSwitch("--sign")]
    public bool? Sign { get; set; }

    [CommandEqualsSeparatorSwitch("--version", SwitchValueSeparator = " ")]
    public string? Version { get; set; }
}
