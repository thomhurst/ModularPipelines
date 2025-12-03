using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Helm.Options;

[CliCommand("package")]
[ExcludeFromCodeCoverage]
public record HelmPackageOptions : HelmOptions
{
    [CliOption("--app-version")]
    public string? AppVersion { get; set; }

    [CliFlag("--dependency-update")]
    public virtual bool? DependencyUpdate { get; set; }

    [CliOption("--destination")]
    public string? Destination { get; set; }

    [CliOption("--key")]
    public string? Key { get; set; }

    [CliOption("--keyring")]
    public string? Keyring { get; set; }

    [CliOption("--passphrase-file")]
    public string? PassphraseFile { get; set; }

    [CliFlag("--sign")]
    public virtual bool? Sign { get; set; }

    [CliOption("--version")]
    public string? Version { get; set; }
}