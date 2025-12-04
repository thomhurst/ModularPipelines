using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("keyvault", "secret", "set-attributes")]
public record AzKeyvaultSecretSetAttributesOptions : AzOptions
{
    [CliOption("--content-type")]
    public string? ContentType { get; set; }

    [CliFlag("--enabled")]
    public bool? Enabled { get; set; }

    [CliOption("--expires")]
    public string? Expires { get; set; }

    [CliOption("--id")]
    public string? Id { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--not-before")]
    public string? NotBefore { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--vault-name")]
    public string? VaultName { get; set; }

    [CliOption("--version")]
    public string? Version { get; set; }
}