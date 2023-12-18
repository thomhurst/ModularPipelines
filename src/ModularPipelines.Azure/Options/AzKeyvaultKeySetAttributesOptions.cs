using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("keyvault", "key", "set-attributes")]
public record AzKeyvaultKeySetAttributesOptions : AzOptions
{
    [BooleanCommandSwitch("--enabled")]
    public bool? Enabled { get; set; }

    [CommandSwitch("--expires")]
    public string? Expires { get; set; }

    [CommandSwitch("--hsm-name")]
    public string? HsmName { get; set; }

    [CommandSwitch("--id")]
    public string? Id { get; set; }

    [BooleanCommandSwitch("--immutable")]
    public bool? Immutable { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--not-before")]
    public string? NotBefore { get; set; }

    [CommandSwitch("--ops")]
    public string? Ops { get; set; }

    [CommandSwitch("--policy")]
    public string? Policy { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--vault-name")]
    public string? VaultName { get; set; }

    [CommandSwitch("--version")]
    public string? Version { get; set; }
}