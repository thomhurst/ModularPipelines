using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datamigration", "sql-vm", "create")]
public record AzDatamigrationSqlVmCreateOptions(
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--sql-vm-name")] string SqlVmName,
[property: CommandSwitch("--target-db-name")] string TargetDbName
) : AzOptions
{
    [CommandSwitch("--migration-service")]
    public string? MigrationService { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--offline-configuration")]
    public string? OfflineConfiguration { get; set; }

    [CommandSwitch("--scope")]
    public string? Scope { get; set; }

    [CommandSwitch("--source-database-name")]
    public string? SourceDatabaseName { get; set; }

    [CommandSwitch("--source-location")]
    public string? SourceLocation { get; set; }

    [CommandSwitch("--source-sql-connection")]
    public string? SourceSqlConnection { get; set; }

    [CommandSwitch("--target-db-collation")]
    public string? TargetDbCollation { get; set; }

    [CommandSwitch("--target-location")]
    public string? TargetLocation { get; set; }
}

