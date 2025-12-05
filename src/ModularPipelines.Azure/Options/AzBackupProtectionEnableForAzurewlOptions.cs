using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("backup", "protection", "enable-for-urewl")]
public record AzBackupProtectionEnableForAzurewlOptions(
[property: CliOption("--policy-name")] string PolicyName,
[property: CliOption("--protectable-item-name")] string ProtectableItemName,
[property: CliOption("--protectable-item-type")] string ProtectableItemType,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--server-name")] string ServerName,
[property: CliOption("--vault-name")] string VaultName,
[property: CliOption("--workload-type")] string WorkloadType
) : AzOptions;