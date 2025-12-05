using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("webapp", "config", "backup", "restore")]
public record AzWebappConfigBackupRestoreOptions(
[property: CliOption("--backup-name")] string BackupName,
[property: CliOption("--container-url")] string ContainerUrl,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--webapp-name")] string WebappName
) : AzOptions
{
    [CliOption("--db-connection-string")]
    public string? DbConnectionString { get; set; }

    [CliOption("--db-name")]
    public string? DbName { get; set; }

    [CliOption("--db-type")]
    public string? DbType { get; set; }

    [CliFlag("--ignore-hostname-conflict")]
    public bool? IgnoreHostnameConflict { get; set; }

    [CliOption("--overwrite")]
    public string? Overwrite { get; set; }

    [CliOption("--slot")]
    public string? Slot { get; set; }

    [CliOption("--target-name")]
    public string? TargetName { get; set; }
}