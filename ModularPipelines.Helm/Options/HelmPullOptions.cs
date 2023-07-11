using ModularPipelines.Attributes;

namespace ModularPipelines.Helm.Options;

[CommandPrecedingArguments("pull")]
public record HelmPullOptions : HelmOptions
{
    [CommandEqualsSeparatorSwitch("--ca-file", SwitchValueSeparator = " ")]
    public string? CaFile { get; set; }

    [CommandEqualsSeparatorSwitch("--cert-file", SwitchValueSeparator = " ")]
    public string? CertFile { get; set; }

    [CommandEqualsSeparatorSwitch("--destination", SwitchValueSeparator = " ")]
    public string? Destination { get; set; }

    [BooleanCommandSwitch("--devel")]
    public bool? Devel { get; set; }

    [BooleanCommandSwitch("--insecure-skip-tls-verify")]
    public bool? InsecureSkipTlsVerify { get; set; }

    [CommandEqualsSeparatorSwitch("--key-file", SwitchValueSeparator = " ")]
    public string? KeyFile { get; set; }

    [CommandEqualsSeparatorSwitch("--keyring", SwitchValueSeparator = " ")]
    public string? Keyring { get; set; }

    [BooleanCommandSwitch("--pass-credentials")]
    public bool? PassCredentials { get; set; }

    [CommandEqualsSeparatorSwitch("--password", SwitchValueSeparator = " ")]
    public string? Password { get; set; }

    [BooleanCommandSwitch("--prov")]
    public bool? Prov { get; set; }

    [CommandEqualsSeparatorSwitch("--repo", SwitchValueSeparator = " ")]
    public string? Repo { get; set; }

    [BooleanCommandSwitch("--untar")]
    public bool? Untar { get; set; }

    [CommandEqualsSeparatorSwitch("--untardir", SwitchValueSeparator = " ")]
    public string? Untardir { get; set; }

    [CommandEqualsSeparatorSwitch("--username", SwitchValueSeparator = " ")]
    public string? Username { get; set; }

    [BooleanCommandSwitch("--verify")]
    public bool? Verify { get; set; }

    [CommandEqualsSeparatorSwitch("--version", SwitchValueSeparator = " ")]
    public string? Version { get; set; }

}
