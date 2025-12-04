using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("webapp", "config", "backup", "create")]
public record AzWebappConfigBackupCreateOptions(
[property: CliOption("--container-url")] string ContainerUrl,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--webapp-name")] string WebappName
) : AzOptions
{
    [CliOption("--backup-name")]
    public string? BackupName { get; set; }

    [CliOption("--db-connection-string")]
    public string? DbConnectionString { get; set; }

    [CliOption("--db-name")]
    public string? DbName { get; set; }

    [CliOption("--db-type")]
    public string? DbType { get; set; }

    [CliOption("--slot")]
    public string? Slot { get; set; }
}