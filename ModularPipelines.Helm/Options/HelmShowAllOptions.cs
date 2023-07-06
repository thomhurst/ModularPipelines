using ModularPipelines.Attributes;

namespace ModularPipelines.Helm.Options;

[CommandPrecedingArguments("show", "all")]
public record HelmShowAllOptions : HelmOptions
{
    [CommandLongSwitch("ca-file", SwitchValueSeparator = " ")]
    public string? CaFile { get; set; }

    [CommandLongSwitch("cert-file", SwitchValueSeparator = " ")]
    public string? CertFile { get; set; }

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

    [CommandLongSwitch("repo", SwitchValueSeparator = " ")]
    public string? Repo { get; set; }

    [CommandLongSwitch("username", SwitchValueSeparator = " ")]
    public string? Username { get; set; }

    [BooleanCommandSwitch("verify")]
    public bool? Verify { get; set; }

    [CommandLongSwitch("version", SwitchValueSeparator = " ")]
    public string? Version { get; set; }

}
