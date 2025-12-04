using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("keyvault", "secret", "set")]
public record AzKeyvaultSecretSetOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--vault-name")] string VaultName
) : AzOptions
{
    [CliOption("--content-type")]
    public string? ContentType { get; set; }

    [CliFlag("--disabled")]
    public bool? Disabled { get; set; }

    [CliOption("--encoding")]
    public string? Encoding { get; set; }

    [CliOption("--expires")]
    public string? Expires { get; set; }

    [CliOption("--file")]
    public string? File { get; set; }

    [CliOption("--not-before")]
    public string? NotBefore { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--value")]
    public string? Value { get; set; }
}