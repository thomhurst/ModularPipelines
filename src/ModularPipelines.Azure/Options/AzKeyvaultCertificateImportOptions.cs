using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("keyvault", "certificate", "import")]
public record AzKeyvaultCertificateImportOptions(
[property: CommandSwitch("--file")] string File,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--vault-name")] string VaultName
) : AzOptions
{
    [BooleanCommandSwitch("--disabled")]
    public bool? Disabled { get; set; }

    [CommandSwitch("--password")]
    public string? Password { get; set; }

    [CommandSwitch("--policy")]
    public string? Policy { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}

