using ModularPipelines.Attributes;

namespace ModularPipelines.Helm.Options;

[CommandPrecedingArguments("pull")]
public record HelmPullOptions : HelmOptions
{
    [CommandLongSwitch("ca-file", SwitchValueSeparator = " ")]
    public string? CaFile { get; set; }

    [CommandLongSwitch("cert-file", SwitchValueSeparator = " ")]
    public string? CertFile { get; set; }

    [CommandLongSwitch("destination", SwitchValueSeparator = " ")]
    public string? Destination { get; set; }

    [BooleanCommandSwitch("devel")]
    public bool? Devel { get; set; }

    [BooleanCommandSwitch("insecure-skip-tls-verify")]
    public bool? InsecureSkipTlsVerify { get; set; }

    [CommandLongSwitch("key-file", SwitchValueSeparator = " ")]
    public string? KeyFile { get; set; }

    [CommandLongSwitch("keyring", SwitchValueSeparator = " ")]
    public string? Keyring { get; set; }

    [BooleanCommandSwitch("pass-credentials")]
    public bool? PassCredentials { get; set; }

    [CommandLongSwitch("password", SwitchValueSeparator = " ")]
    public string? Password { get; set; }

    [BooleanCommandSwitch("prov")]
    public bool? Prov { get; set; }

    [CommandLongSwitch("repo", SwitchValueSeparator = " ")]
    public string? Repo { get; set; }

    [BooleanCommandSwitch("untar")]
    public bool? Untar { get; set; }

    [CommandLongSwitch("untardir", SwitchValueSeparator = " ")]
    public string? Untardir { get; set; }

    [CommandLongSwitch("username", SwitchValueSeparator = " ")]
    public string? Username { get; set; }

    [BooleanCommandSwitch("verify")]
    public bool? Verify { get; set; }

    [CommandLongSwitch("version", SwitchValueSeparator = " ")]
    public string? Version { get; set; }

}
