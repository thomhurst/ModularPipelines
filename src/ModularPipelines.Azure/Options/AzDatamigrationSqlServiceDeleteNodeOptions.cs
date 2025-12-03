using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datamigration", "sql-service", "delete-node")]
public record AzDatamigrationSqlServiceDeleteNodeOptions : AzOptions
{
    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--ir-name")]
    public string? IrName { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--node-name")]
    public string? NodeName { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}