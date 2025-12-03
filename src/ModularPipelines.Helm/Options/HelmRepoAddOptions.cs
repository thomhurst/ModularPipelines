using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Helm.Options;

[CliCommand("repo", "add")]
[ExcludeFromCodeCoverage]
public record HelmRepoAddOptions : HelmOptions
{
    [CliFlag("--allow-deprecated-repos")]
    public virtual bool? AllowDeprecatedRepos { get; set; }

    [CliOption("--ca-file")]
    public string? CaFile { get; set; }

    [CliOption("--cert-file")]
    public string? CertFile { get; set; }

    [CliFlag("--force-update")]
    public virtual bool? ForceUpdate { get; set; }

    [CliFlag("--insecure-skip-tls-verify")]
    public virtual bool? InsecureSkipTlsVerify { get; set; }

    [CliOption("--key-file")]
    public string? KeyFile { get; set; }

    [CliFlag("--no-update")]
    public virtual bool? NoUpdate { get; set; }

    [CliFlag("--pass-credentials")]
    public virtual bool? PassCredentials { get; set; }

    [CliOption("--password")]
    public string? Password { get; set; }

    [CliFlag("--password-stdin")]
    public virtual bool? PasswordStdin { get; set; }

    [CliOption("--username")]
    public string? Username { get; set; }
}