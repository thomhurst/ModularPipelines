using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("cosmosdb", "gremlin", "retrieve-latest-backup-time")]
public record AzCosmosdbGremlinRetrieveLatestBackupTimeOptions(
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--database-name")] string DatabaseName,
[property: CliOption("--graph-name")] string GraphName,
[property: CliOption("--location")] string Location,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;