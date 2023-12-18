using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("backup", "protectable-item", "initialize")]
public record AzBackupProtectableItemInitializeOptions(
[property: CommandSwitch("--container-name")] string ContainerName,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--vault-name")] string VaultName,
[property: CommandSwitch("--workload-type")] string WorkloadType
) : AzOptions;