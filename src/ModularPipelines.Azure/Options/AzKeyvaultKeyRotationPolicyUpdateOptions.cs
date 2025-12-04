using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("keyvault", "key", "rotation-policy", "update")]
public record AzKeyvaultKeyRotationPolicyUpdateOptions(
[property: CliOption("--value")] string Value
) : AzOptions
{
    [CliOption("--hsm-name")]
    public string? HsmName { get; set; }

    [CliOption("--id")]
    public string? Id { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--vault-name")]
    public string? VaultName { get; set; }
}