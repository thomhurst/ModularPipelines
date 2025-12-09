using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("webapp", "config", "backup", "delete")]
public record AzWebappConfigBackupDeleteOptions(
[property: CliOption("--backup-id")] string BackupId,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--webapp-name")] string WebappName
) : AzOptions
{
    [CliOption("--slot")]
    public string? Slot { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}