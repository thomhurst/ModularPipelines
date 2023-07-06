using ModularPipelines.Attributes;

namespace ModularPipelines.Helm.Options;

[CommandPrecedingArguments("repo", "add")]
public record HelmRepoAddOptions : HelmOptions
{
    [BooleanCommandSwitch("allow-deprecated-repos")]
    public bool? AllowDeprecatedRepos { get; set; }

    [CommandLongSwitch("ca-file", SwitchValueSeparator = " ")]
    public string? CaFile { get; set; }

    [CommandLongSwitch("cert-file", SwitchValueSeparator = " ")]
    public string? CertFile { get; set; }

    [BooleanCommandSwitch("force-update")]
    public bool? ForceUpdate { get; set; }

    [BooleanCommandSwitch("insecure-skip-tls-verify")]
    public bool? InsecureSkipTlsVerify { get; set; }

    [CommandLongSwitch("key-file", SwitchValueSeparator = " ")]
    public string? KeyFile { get; set; }

    [BooleanCommandSwitch("no-update")]
    public bool? NoUpdate { get; set; }

    [BooleanCommandSwitch("pass-credentials")]
    public bool? PassCredentials { get; set; }

    [CommandLongSwitch("password", SwitchValueSeparator = " ")]
    public string? Password { get; set; }

    [BooleanCommandSwitch("password-stdin")]
    public bool? PasswordStdin { get; set; }

    [CommandLongSwitch("username", SwitchValueSeparator = " ")]
    public string? Username { get; set; }

}
