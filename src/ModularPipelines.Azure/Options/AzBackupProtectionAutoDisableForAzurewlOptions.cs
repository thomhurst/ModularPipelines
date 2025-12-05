using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("backup", "protection", "auto-disable-for-urewl")]
public record AzBackupProtectionAutoDisableForAzurewlOptions(
[property: CliOption("--protectable-item-name")] string ProtectableItemName,
[property: CliOption("--protectable-item-type")] string ProtectableItemType,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--server-name")] string ServerName,
[property: CliOption("--vault-name")] string VaultName,
[property: CliOption("--workload-type")] string WorkloadType
) : AzOptions;