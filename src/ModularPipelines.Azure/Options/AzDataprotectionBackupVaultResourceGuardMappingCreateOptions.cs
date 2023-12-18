using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataprotection", "backup-vault", "resource-guard-mapping", "create")]
public record AzDataprotectionBackupVaultResourceGuardMappingCreateOptions(
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--vault-name")] string VaultName
) : AzOptions
{
    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--resource-guard-id")]
    public string? ResourceGuardId { get; set; }
}