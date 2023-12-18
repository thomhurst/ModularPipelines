using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datamigration", "sql-managed-instance", "show")]
public record AzDatamigrationSqlManagedInstanceShowOptions : AzOptions
{
    [CommandSwitch("--expand")]
    public string? Expand { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--managed-instance-name")]
    public string? ManagedInstanceName { get; set; }

    [CommandSwitch("--migration-operation-id")]
    public string? MigrationOperationId { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [CommandSwitch("--target-db-name")]
    public string? TargetDbName { get; set; }
}