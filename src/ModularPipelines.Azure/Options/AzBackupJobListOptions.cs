using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("backup", "job", "list")]
public record AzBackupJobListOptions(
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--vault-name")] string VaultName
) : AzOptions
{
    [CliOption("--backup-management-type")]
    public string? BackupManagementType { get; set; }

    [CliOption("--end-date")]
    public string? EndDate { get; set; }

    [CliOption("--operation")]
    public string? Operation { get; set; }

    [CliOption("--start-date")]
    public string? StartDate { get; set; }

    [CliOption("--status")]
    public string? Status { get; set; }

    [CliFlag("--use-secondary-region")]
    public bool? UseSecondaryRegion { get; set; }
}