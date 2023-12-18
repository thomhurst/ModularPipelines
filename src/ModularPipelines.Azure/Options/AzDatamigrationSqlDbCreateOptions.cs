using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datamigration", "sql-db", "create")]
public record AzDatamigrationSqlDbCreateOptions(
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--sqldb-instance-name")] string SqldbInstanceName,
[property: CommandSwitch("--target-db-name")] string TargetDbName
) : AzOptions
{
    [CommandSwitch("--migration-service")]
    public string? MigrationService { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--scope")]
    public string? Scope { get; set; }

    [CommandSwitch("--source-database-name")]
    public string? SourceDatabaseName { get; set; }

    [CommandSwitch("--source-sql-connection")]
    public string? SourceSqlConnection { get; set; }

    [CommandSwitch("--table-list")]
    public string? TableList { get; set; }

    [CommandSwitch("--target-db-collation")]
    public string? TargetDbCollation { get; set; }

    [CommandSwitch("--target-sql-connection")]
    public string? TargetSqlConnection { get; set; }
}

