using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("cosmosdb", "sql", "container", "throughput", "migrate")]
public record AzCosmosdbSqlContainerThroughputMigrateOptions(
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--database-name")] string DatabaseName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--throughput-type")] string ThroughputType
) : AzOptions;