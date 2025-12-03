using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("keyvault", "key", "set-attributes")]
public record AzKeyvaultKeySetAttributesOptions : AzOptions
{
    [CliFlag("--enabled")]
    public bool? Enabled { get; set; }

    [CliOption("--expires")]
    public string? Expires { get; set; }

    [CliOption("--hsm-name")]
    public string? HsmName { get; set; }

    [CliOption("--id")]
    public string? Id { get; set; }

    [CliFlag("--immutable")]
    public bool? Immutable { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--not-before")]
    public string? NotBefore { get; set; }

    [CliOption("--ops")]
    public string? Ops { get; set; }

    [CliOption("--policy")]
    public string? Policy { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--vault-name")]
    public string? VaultName { get; set; }

    [CliOption("--version")]
    public string? Version { get; set; }
}