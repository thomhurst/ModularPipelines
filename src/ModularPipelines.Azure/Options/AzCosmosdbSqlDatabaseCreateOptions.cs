using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cosmosdb", "sql", "database", "create")]
public record AzCosmosdbSqlDatabaseCreateOptions(
[property: CommandSwitch("--account-name")] int AccountName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--max-throughput")]
    public string? MaxThroughput { get; set; }

    [CommandSwitch("--throughput")]
    public string? Throughput { get; set; }
}