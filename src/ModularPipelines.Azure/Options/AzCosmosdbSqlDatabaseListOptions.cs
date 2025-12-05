using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("cosmosdb", "sql", "database", "list")]
public record AzCosmosdbSqlDatabaseListOptions(
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;