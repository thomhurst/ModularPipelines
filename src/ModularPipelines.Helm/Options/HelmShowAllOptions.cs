using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Helm.Options;

[CliCommand("show", "all")]
[ExcludeFromCodeCoverage]
public record HelmShowAllOptions : HelmOptions
{
    [CliOption("--ca-file")]
    public virtual string? CaFile { get; set; }

    [CliOption("--cert-file")]
    public virtual string? CertFile { get; set; }

    [CliFlag("--devel")]
    public virtual bool? Devel { get; set; }

    [CliFlag("--insecure-skip-tls-verify")]
    public virtual bool? InsecureSkipTlsVerify { get; set; }

    [CliOption("--key-file")]
    public virtual string? KeyFile { get; set; }

    [CliOption("--keyring")]
    public virtual string? Keyring { get; set; }

    [CliFlag("--pass-credentials")]
    public virtual bool? PassCredentials { get; set; }

    [CliOption("--password")]
    public virtual string? Password { get; set; }

    [CliOption("--repo")]
    public virtual string? Repo { get; set; }

    [CliOption("--username")]
    public virtual string? Username { get; set; }

    [CliFlag("--verify")]
    public virtual bool? Verify { get; set; }

    [CliOption("--version")]
    public virtual string? Version { get; set; }
}