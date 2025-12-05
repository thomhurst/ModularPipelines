using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Helm.Options;

[CliCommand("package")]
[ExcludeFromCodeCoverage]
public record HelmPackageOptions : HelmOptions
{
    [CliOption("--app-version")]
    public virtual string? AppVersion { get; set; }

    [CliFlag("--dependency-update")]
    public virtual bool? DependencyUpdate { get; set; }

    [CliOption("--destination")]
    public virtual string? Destination { get; set; }

    [CliOption("--key")]
    public virtual string? Key { get; set; }

    [CliOption("--keyring")]
    public virtual string? Keyring { get; set; }

    [CliOption("--passphrase-file")]
    public virtual string? PassphraseFile { get; set; }

    [CliFlag("--sign")]
    public virtual bool? Sign { get; set; }

    [CliOption("--version")]
    public virtual string? Version { get; set; }
}