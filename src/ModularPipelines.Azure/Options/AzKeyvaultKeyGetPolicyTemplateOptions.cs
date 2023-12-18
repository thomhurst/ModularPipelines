using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("keyvault", "key", "get-policy-template")]
public record AzKeyvaultKeyGetPolicyTemplateOptions(
[property: CommandSwitch("--count")] int Count
) : AzOptions
{
    [CommandSwitch("--byok-file")]
    public string? ByokFile { get; set; }

    [CommandSwitch("--byok-string")]
    public string? ByokString { get; set; }

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

    [CommandSwitch("--pem-file")]
    public string? PemFile { get; set; }

    [CommandSwitch("--pem-password")]
    public string? PemPassword { get; set; }

    [CommandSwitch("--pem-string")]
    public string? PemString { get; set; }

    [CommandSwitch("--policy")]
    public string? Policy { get; set; }

    [CommandSwitch("--protection")]
    public string? Protection { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--vault-name")]
    public string? VaultName { get; set; }
}

