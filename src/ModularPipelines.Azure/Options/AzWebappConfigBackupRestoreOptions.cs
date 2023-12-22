using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("webapp", "config", "backup", "restore")]
public record AzWebappConfigBackupRestoreOptions(
[property: CommandSwitch("--backup-name")] string BackupName,
[property: CommandSwitch("--container-url")] string ContainerUrl,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--webapp-name")] string WebappName
) : AzOptions
{
    [CommandSwitch("--db-connection-string")]
    public string? DbConnectionString { get; set; }

    [CommandSwitch("--db-name")]
    public string? DbName { get; set; }

    [CommandSwitch("--db-type")]
    public string? DbType { get; set; }

    [BooleanCommandSwitch("--ignore-hostname-conflict")]
    public bool? IgnoreHostnameConflict { get; set; }

    [CommandSwitch("--overwrite")]
    public string? Overwrite { get; set; }

    [CommandSwitch("--slot")]
    public string? Slot { get; set; }

    [CommandSwitch("--target-name")]
    public string? TargetName { get; set; }
}