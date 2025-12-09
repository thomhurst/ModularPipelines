using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("keyvault", "certificate", "create")]
public record AzKeyvaultCertificateCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--policy")] string Policy,
[property: CliOption("--vault-name")] string VaultName
) : AzOptions
{
    [CliFlag("--disabled")]
    public bool? Disabled { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--validity")]
    public string? Validity { get; set; }
}