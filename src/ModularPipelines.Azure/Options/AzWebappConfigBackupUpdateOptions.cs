using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("webapp", "config", "backup", "update")]
public record AzWebappConfigBackupUpdateOptions(
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--webapp-name")] string WebappName
) : AzOptions
{
    [CommandSwitch("--backup-name")]
    public string? BackupName { get; set; }

    [CommandSwitch("--container-url")]
    public string? ContainerUrl { get; set; }

    [CommandSwitch("--db-connection-string")]
    public string? DbConnectionString { get; set; }

    [CommandSwitch("--db-name")]
    public string? DbName { get; set; }

    [CommandSwitch("--db-type")]
    public string? DbType { get; set; }

    [CommandSwitch("--frequency")]
    public string? Frequency { get; set; }

    [BooleanCommandSwitch("--retain-one")]
    public bool? RetainOne { get; set; }

    [CommandSwitch("--retention")]
    public string? Retention { get; set; }

    [CommandSwitch("--slot")]
    public string? Slot { get; set; }
}