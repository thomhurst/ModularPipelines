using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("cosmosdb", "sql", "container", "show")]
public record AzCosmosdbSqlContainerShowOptions(
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--database-name")] string DatabaseName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;