using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datamigration", "sql-service", "delete-node")]
public record AzDatamigrationSqlServiceDeleteNodeOptions : AzOptions
{
    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--ir-name")]
    public string? IrName { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--node-name")]
    public string? NodeName { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}