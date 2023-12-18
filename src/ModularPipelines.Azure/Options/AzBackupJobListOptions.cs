using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("backup", "job", "list")]
public record AzBackupJobListOptions(
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--vault-name")] string VaultName
) : AzOptions
{
    [CommandSwitch("--backup-management-type")]
    public string? BackupManagementType { get; set; }

    [CommandSwitch("--end-date")]
    public string? EndDate { get; set; }

    [CommandSwitch("--operation")]
    public string? Operation { get; set; }

    [CommandSwitch("--start-date")]
    public string? StartDate { get; set; }

    [CommandSwitch("--status")]
    public string? Status { get; set; }

    [BooleanCommandSwitch("--use-secondary-region")]
    public bool? UseSecondaryRegion { get; set; }
}