using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("keyvault", "certificate", "import")]
public record AzKeyvaultCertificateImportOptions(
[property: CliOption("--file")] string File,
[property: CliOption("--name")] string Name,
[property: CliOption("--vault-name")] string VaultName
) : AzOptions
{
    [CliFlag("--disabled")]
    public bool? Disabled { get; set; }

    [CliOption("--password")]
    public string? Password { get; set; }

    [CliOption("--policy")]
    public string? Policy { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}