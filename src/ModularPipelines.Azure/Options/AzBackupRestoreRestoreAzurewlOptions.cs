using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("backup", "restore", "restore-urewl")]
public record AzBackupRestoreRestoreAzurewlOptions(
[property: CommandSwitch("--recovery-config")] string RecoveryConfig,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--vault-name")] string VaultName
) : AzOptions
{
    [CommandSwitch("--rehydration-duration")]
    public string? RehydrationDuration { get; set; }

    [CommandSwitch("--rehydration-priority")]
    public string? RehydrationPriority { get; set; }

    [BooleanCommandSwitch("--use-secondary-region")]
    public bool? UseSecondaryRegion { get; set; }
}