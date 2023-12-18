using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cosmosdb", "dts", "cancel")]
public record AzCosmosdbDtsCancelOptions(
[property: CommandSwitch("--account-name")] int AccountName,
[property: CommandSwitch("--job-name")] string JobName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--dest-cassandra-table")]
    public string? DestCassandraTable { get; set; }

    [CommandSwitch("--dest-mongo")]
    public string? DestMongo { get; set; }

    [CommandSwitch("--dest-sql-container")]
    public string? DestSqlContainer { get; set; }

    [CommandSwitch("--source-cassandra-table")]
    public string? SourceCassandraTable { get; set; }

    [CommandSwitch("--source-mongo")]
    public string? SourceMongo { get; set; }

    [CommandSwitch("--source-sql-container")]
    public string? SourceSqlContainer { get; set; }

    [CommandSwitch("--worker-count")]
    public int? WorkerCount { get; set; }
}