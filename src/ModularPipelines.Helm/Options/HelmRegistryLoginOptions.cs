using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Helm.Options;

[CliCommand("registry", "login")]
[ExcludeFromCodeCoverage]
public record HelmRegistryLoginOptions : HelmOptions
{
    [CliOption("--ca-file")]
    public string? CaFile { get; set; }

    [CliOption("--cert-file")]
    public string? CertFile { get; set; }

    [CliFlag("--insecure")]
    public virtual bool? Insecure { get; set; }

    [CliOption("--key-file")]
    public string? KeyFile { get; set; }

    [CliOption("--password")]
    public string? Password { get; set; }

    [CliFlag("--password-stdin")]
    public virtual bool? PasswordStdin { get; set; }

    [CliOption("--username")]
    public string? Username { get; set; }
}