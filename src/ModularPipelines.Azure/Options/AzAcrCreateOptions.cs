using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("acr", "create")]
public record AzAcrCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--sku")] string Sku
) : AzOptions
{
    [CliFlag("--admin-enabled")]
    public bool? AdminEnabled { get; set; }

    [CliFlag("--allow-exports")]
    public bool? AllowExports { get; set; }

    [CliFlag("--allow-trusted-services")]
    public bool? AllowTrustedServices { get; set; }

    [CliOption("--default-action")]
    public string? DefaultAction { get; set; }

    [CliOption("--identity")]
    public string? Identity { get; set; }

    [CliOption("--key-encryption-key")]
    public string? KeyEncryptionKey { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliFlag("--public-network-enabled")]
    public bool? PublicNetworkEnabled { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--workspace")]
    public string? Workspace { get; set; }

    [CliOption("--zone-redundancy")]
    public string? ZoneRedundancy { get; set; }
}