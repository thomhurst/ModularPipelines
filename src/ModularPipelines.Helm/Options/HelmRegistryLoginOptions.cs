using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Helm.Options;

[CliCommand("registry", "login")]
[ExcludeFromCodeCoverage]
public record HelmRegistryLoginOptions : HelmOptions
{
    [CliOption("--ca-file")]
    public virtual string? CaFile { get; set; }

    [CliOption("--cert-file")]
    public virtual string? CertFile { get; set; }

    [CliFlag("--insecure")]
    public virtual bool? Insecure { get; set; }

    [CliOption("--key-file")]
    public virtual string? KeyFile { get; set; }

    [CliOption("--password")]
    public virtual string? Password { get; set; }

    [CliFlag("--password-stdin")]
    public virtual bool? PasswordStdin { get; set; }

    [CliOption("--username")]
    public virtual string? Username { get; set; }
}