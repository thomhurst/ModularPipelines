using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

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

    [CommandSwitch("--use-secondary-region")]
    public string? UseSecondaryRegion { get; set; }
}

