using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("keyvault", "secret", "backup")]
public record AzKeyvaultSecretBackupOptions(
[property: CommandSwitch("--file")] string File
) : AzOptions
{
    [CommandSwitch("--id")]
    public string? Id { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--vault-name")]
    public string? VaultName { get; set; }
}