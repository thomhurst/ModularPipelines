using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("keyvault", "certificate", "restore")]
public record AzKeyvaultCertificateRestoreOptions(
[property: CommandSwitch("--file")] string File
) : AzOptions
{
    [CommandSwitch("--vault-name")]
    public string? VaultName { get; set; }
}