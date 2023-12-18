using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("keyvault", "secret", "restore")]
public record AzKeyvaultSecretRestoreOptions(
[property: CommandSwitch("--file")] string File,
[property: CommandSwitch("--vault-name")] string VaultName
) : AzOptions
{
    [CommandSwitch("--content-type")]
    public string? ContentType { get; set; }

    [BooleanCommandSwitch("--disabled")]
    public bool? Disabled { get; set; }

    [CommandSwitch("--encoding")]
    public string? Encoding { get; set; }

    [CommandSwitch("--expires")]
    public string? Expires { get; set; }

    [CommandSwitch("--not-before")]
    public string? NotBefore { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--value")]
    public string? Value { get; set; }
}

