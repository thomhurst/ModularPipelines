using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datamigration", "sql-service", "delete-node")]
public record AzDatamigrationSqlServiceDeleteNodeOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--ir-name")]
    public string? IrName { get; set; }

    [CommandSwitch("--node-name")]
    public string? NodeName { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}