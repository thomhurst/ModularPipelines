using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("keyvault", "key", "decrypt")]
public record AzKeyvaultKeyDecryptOptions(
[property: CliOption("--algorithm")] string Algorithm,
[property: CliOption("--value")] string Value
) : AzOptions
{
    [CliOption("--aad")]
    public string? Aad { get; set; }

    [CliOption("--data-type")]
    public string? DataType { get; set; }

    [CliOption("--hsm-name")]
    public string? HsmName { get; set; }

    [CliOption("--id")]
    public string? Id { get; set; }

    [CliOption("--iv")]
    public string? Iv { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--tag")]
    public string? Tag { get; set; }

    [CliOption("--vault-name")]
    public string? VaultName { get; set; }

    [CliOption("--version")]
    public string? Version { get; set; }
}