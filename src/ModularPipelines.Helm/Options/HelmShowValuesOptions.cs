using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Helm.Options;

[CliCommand("show", "values")]
[ExcludeFromCodeCoverage]
public record HelmShowValuesOptions : HelmOptions
{
    [CliOption("--ca-file")]
    public string? CaFile { get; set; }

    [CliOption("--cert-file")]
    public string? CertFile { get; set; }

    [CliFlag("--devel")]
    public virtual bool? Devel { get; set; }

    [CliFlag("--insecure-skip-tls-verify")]
    public virtual bool? InsecureSkipTlsVerify { get; set; }

    [CliOption("--jsonpath")]
    public string? Jsonpath { get; set; }

    [CliOption("--key-file")]
    public string? KeyFile { get; set; }

    [CliOption("--keyring")]
    public string? Keyring { get; set; }

    [CliFlag("--pass-credentials")]
    public virtual bool? PassCredentials { get; set; }

    [CliOption("--password")]
    public string? Password { get; set; }

    [CliOption("--repo")]
    public string? Repo { get; set; }

    [CliOption("--username")]
    public string? Username { get; set; }

    [CliFlag("--verify")]
    public virtual bool? Verify { get; set; }

    [CliOption("--version")]
    public string? Version { get; set; }
}