using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Helm.Options;

[CliCommand("push")]
[ExcludeFromCodeCoverage]
public record HelmPushOptions : HelmOptions
{
    [CliOption("--ca-file")]
    public virtual string? CaFile { get; set; }

    [CliOption("--cert-file")]
    public virtual string? CertFile { get; set; }

    [CliFlag("--insecure-skip-tls-verify")]
    public virtual bool? InsecureSkipTlsVerify { get; set; }

    [CliOption("--key-file")]
    public virtual string? KeyFile { get; set; }
}