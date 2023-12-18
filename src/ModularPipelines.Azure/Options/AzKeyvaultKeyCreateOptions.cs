using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("keyvault", "key", "create")]
public record AzKeyvaultKeyCreateOptions : AzOptions
{
    [CommandSwitch("--curve")]
    public string? Curve { get; set; }

    [BooleanCommandSwitch("--default-cvm-policy")]
    public bool? DefaultCvmPolicy { get; set; }

    [BooleanCommandSwitch("--disabled")]
    public bool? Disabled { get; set; }

    [CommandSwitch("--expires")]
    public string? Expires { get; set; }

    [BooleanCommandSwitch("--exportable")]
    public bool? Exportable { get; set; }

    [CommandSwitch("--hsm-name")]
    public string? HsmName { get; set; }

    [CommandSwitch("--id")]
    public string? Id { get; set; }

    [BooleanCommandSwitch("--immutable")]
    public bool? Immutable { get; set; }

    [CommandSwitch("--kty")]
    public string? Kty { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--not-before")]
    public string? NotBefore { get; set; }

    [CommandSwitch("--ops")]
    public string? Ops { get; set; }

    [CommandSwitch("--policy")]
    public string? Policy { get; set; }

    [CommandSwitch("--protection")]
    public string? Protection { get; set; }

    [CommandSwitch("--size")]
    public string? Size { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--vault-name")]
    public string? VaultName { get; set; }
}