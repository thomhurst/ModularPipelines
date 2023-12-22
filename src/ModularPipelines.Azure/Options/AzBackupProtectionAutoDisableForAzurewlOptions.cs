using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("backup", "protection", "auto-disable-for-urewl")]
public record AzBackupProtectionAutoDisableForAzurewlOptions(
[property: CommandSwitch("--protectable-item-name")] string ProtectableItemName,
[property: CommandSwitch("--protectable-item-type")] string ProtectableItemType,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--server-name")] string ServerName,
[property: CommandSwitch("--vault-name")] string VaultName,
[property: CommandSwitch("--workload-type")] string WorkloadType
) : AzOptions;