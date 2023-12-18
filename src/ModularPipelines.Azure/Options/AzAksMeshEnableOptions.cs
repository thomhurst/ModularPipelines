using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("aks", "mesh", "enable")]
public record AzAksMeshEnableOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--ca-cert-object-name")]
    public string? CaCertObjectName { get; set; }

    [CommandSwitch("--ca-key-object-name")]
    public string? CaKeyObjectName { get; set; }

    [CommandSwitch("--cert-chain-object-name")]
    public string? CertChainObjectName { get; set; }

    [CommandSwitch("--key-vault-id")]
    public string? KeyVaultId { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--revision")]
    public string? Revision { get; set; }

    [CommandSwitch("--root-cert-object-name")]
    public string? RootCertObjectName { get; set; }
}

