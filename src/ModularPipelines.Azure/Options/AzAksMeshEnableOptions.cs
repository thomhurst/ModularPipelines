using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("aks", "mesh", "enable")]
public record AzAksMeshEnableOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--ca-cert-object-name")]
    public string? CaCertObjectName { get; set; }

    [CliOption("--ca-key-object-name")]
    public string? CaKeyObjectName { get; set; }

    [CliOption("--cert-chain-object-name")]
    public string? CertChainObjectName { get; set; }

    [CliOption("--key-vault-id")]
    public string? KeyVaultId { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--revision")]
    public string? Revision { get; set; }

    [CliOption("--root-cert-object-name")]
    public string? RootCertObjectName { get; set; }
}