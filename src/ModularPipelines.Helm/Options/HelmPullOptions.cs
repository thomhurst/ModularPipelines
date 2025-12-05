using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Helm.Options;

[CliCommand("pull")]
[ExcludeFromCodeCoverage]
public record HelmPullOptions : HelmOptions
{
    [CliOption("--ca-file")]
    public virtual string? CaFile { get; set; }

    [CliOption("--cert-file")]
    public virtual string? CertFile { get; set; }

    [CliOption("--destination")]
    public virtual string? Destination { get; set; }

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

    [CliFlag("--prov")]
    public virtual bool? Prov { get; set; }

    [CliOption("--repo")]
    public virtual string? Repo { get; set; }

    [CliFlag("--untar")]
    public virtual bool? Untar { get; set; }

    [CliOption("--untardir")]
    public virtual string? Untardir { get; set; }

    [CliOption("--username")]
    public virtual string? Username { get; set; }

    [CliFlag("--verify")]
    public virtual bool? Verify { get; set; }

    [CliOption("--version")]
    public virtual string? Version { get; set; }
}