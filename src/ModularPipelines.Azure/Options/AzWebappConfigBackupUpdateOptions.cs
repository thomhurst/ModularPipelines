using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("webapp", "config", "backup", "update")]
public record AzWebappConfigBackupUpdateOptions(
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--webapp-name")] string WebappName
) : AzOptions
{
    [CliOption("--backup-name")]
    public string? BackupName { get; set; }

    [CliOption("--container-url")]
    public string? ContainerUrl { get; set; }

    [CliOption("--db-connection-string")]
    public string? DbConnectionString { get; set; }

    [CliOption("--db-name")]
    public string? DbName { get; set; }

    [CliOption("--db-type")]
    public string? DbType { get; set; }

    [CliOption("--frequency")]
    public string? Frequency { get; set; }

    [CliFlag("--retain-one")]
    public bool? RetainOne { get; set; }

    [CliOption("--retention")]
    public string? Retention { get; set; }

    [CliOption("--slot")]
    public string? Slot { get; set; }
}