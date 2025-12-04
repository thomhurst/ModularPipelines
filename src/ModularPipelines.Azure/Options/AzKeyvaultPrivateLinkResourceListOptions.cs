using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("keyvault", "private-link-resource", "list")]
public record AzKeyvaultPrivateLinkResourceListOptions : AzOptions
{
    [CliOption("--hsm-name")]
    public string? HsmName { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--vault-name")]
    public string? VaultName { get; set; }
}