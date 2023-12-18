using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Helm.Options;

[CommandPrecedingArguments("repo", "add")]
[ExcludeFromCodeCoverage]
public record HelmRepoAddOptions : HelmOptions
{
    [BooleanCommandSwitch("--allow-deprecated-repos")]
    public bool? AllowDeprecatedRepos { get; set; }

    [CommandEqualsSeparatorSwitch("--ca-file", SwitchValueSeparator = " ")]
    public string? CaFile { get; set; }

    [CommandEqualsSeparatorSwitch("--cert-file", SwitchValueSeparator = " ")]
    public string? CertFile { get; set; }

    [BooleanCommandSwitch("--force-update")]
    public bool? ForceUpdate { get; set; }

    [BooleanCommandSwitch("--insecure-skip-tls-verify")]
    public bool? InsecureSkipTlsVerify { get; set; }

    [CommandEqualsSeparatorSwitch("--key-file", SwitchValueSeparator = " ")]
    public string? KeyFile { get; set; }

    [BooleanCommandSwitch("--no-update")]
    public bool? NoUpdate { get; set; }

    [BooleanCommandSwitch("--pass-credentials")]
    public bool? PassCredentials { get; set; }

    [CommandEqualsSeparatorSwitch("--password", SwitchValueSeparator = " ")]
    public string? Password { get; set; }

    [BooleanCommandSwitch("--password-stdin")]
    public bool? PasswordStdin { get; set; }

    [CommandEqualsSeparatorSwitch("--username", SwitchValueSeparator = " ")]
    public string? Username { get; set; }
}