using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("backup", "protectable-item", "initialize")]
public record AzBackupProtectableItemInitializeOptions(
[property: CliOption("--container-name")] string ContainerName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--vault-name")] string VaultName,
[property: CliOption("--workload-type")] string WorkloadType
) : AzOptions;