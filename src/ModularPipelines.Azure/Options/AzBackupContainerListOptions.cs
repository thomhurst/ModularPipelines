using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("backup", "container", "list")]
public record AzBackupContainerListOptions(
[property: CliOption("--backup-management-type")] string BackupManagementType,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--vault-name")] string VaultName
) : AzOptions
{
    [CliFlag("--use-secondary-region")]
    public bool? UseSecondaryRegion { get; set; }
}