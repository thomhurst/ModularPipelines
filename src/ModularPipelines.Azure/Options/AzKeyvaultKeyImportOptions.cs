using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("keyvault", "key", "import")]
public record AzKeyvaultKeyImportOptions : AzOptions
{
    [CliOption("--byok-file")]
    public string? ByokFile { get; set; }

    [CliOption("--byok-string")]
    public string? ByokString { get; set; }

    [CliOption("--curve")]
    public string? Curve { get; set; }

    [CliFlag("--default-cvm-policy")]
    public bool? DefaultCvmPolicy { get; set; }

    [CliFlag("--disabled")]
    public bool? Disabled { get; set; }

    [CliOption("--expires")]
    public string? Expires { get; set; }

    [CliFlag("--exportable")]
    public bool? Exportable { get; set; }

    [CliOption("--hsm-name")]
    public string? HsmName { get; set; }

    [CliOption("--id")]
    public string? Id { get; set; }

    [CliFlag("--immutable")]
    public bool? Immutable { get; set; }

    [CliOption("--kty")]
    public string? Kty { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--not-before")]
    public string? NotBefore { get; set; }

    [CliOption("--ops")]
    public string? Ops { get; set; }

    [CliOption("--pem-file")]
    public string? PemFile { get; set; }

    [CliOption("--pem-password")]
    public string? PemPassword { get; set; }

    [CliOption("--pem-string")]
    public string? PemString { get; set; }

    [CliOption("--policy")]
    public string? Policy { get; set; }

    [CliOption("--protection")]
    public string? Protection { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--vault-name")]
    public string? VaultName { get; set; }
}