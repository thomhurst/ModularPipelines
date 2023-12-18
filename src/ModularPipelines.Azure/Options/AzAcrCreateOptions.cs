using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("acr", "create")]
public record AzAcrCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--sku")] string Sku
) : AzOptions
{
    [BooleanCommandSwitch("--admin-enabled")]
    public bool? AdminEnabled { get; set; }

    [BooleanCommandSwitch("--allow-exports")]
    public bool? AllowExports { get; set; }

    [BooleanCommandSwitch("--allow-trusted-services")]
    public bool? AllowTrustedServices { get; set; }

    [CommandSwitch("--default-action")]
    public string? DefaultAction { get; set; }

    [CommandSwitch("--identity")]
    public string? Identity { get; set; }

    [CommandSwitch("--key-encryption-key")]
    public string? KeyEncryptionKey { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [BooleanCommandSwitch("--public-network-enabled")]
    public bool? PublicNetworkEnabled { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--workspace")]
    public string? Workspace { get; set; }

    [CommandSwitch("--zone-redundancy")]
    public string? ZoneRedundancy { get; set; }
}