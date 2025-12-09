using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("backup", "restore", "restore-urewl")]
public record AzBackupRestoreRestoreAzurewlOptions(
[property: CliOption("--recovery-config")] string RecoveryConfig,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--vault-name")] string VaultName
) : AzOptions
{
    [CliOption("--rehydration-duration")]
    public string? RehydrationDuration { get; set; }

    [CliOption("--rehydration-priority")]
    public string? RehydrationPriority { get; set; }

    [CliFlag("--use-secondary-region")]
    public bool? UseSecondaryRegion { get; set; }
}