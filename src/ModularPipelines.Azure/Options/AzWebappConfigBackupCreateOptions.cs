using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("webapp", "config", "backup", "create")]
public record AzWebappConfigBackupCreateOptions(
[property: CommandSwitch("--container-url")] string ContainerUrl,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--webapp-name")] string WebappName
) : AzOptions
{
    [CommandSwitch("--backup-name")]
    public string? BackupName { get; set; }

    [CommandSwitch("--db-connection-string")]
    public string? DbConnectionString { get; set; }

    [CommandSwitch("--db-name")]
    public string? DbName { get; set; }

    [CommandSwitch("--db-type")]
    public string? DbType { get; set; }

    [CommandSwitch("--slot")]
    public string? Slot { get; set; }
}

