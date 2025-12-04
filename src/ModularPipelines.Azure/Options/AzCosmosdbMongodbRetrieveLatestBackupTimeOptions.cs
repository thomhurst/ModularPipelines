using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("cosmosdb", "mongodb", "retrieve-latest-backup-time")]
public record AzCosmosdbMongodbRetrieveLatestBackupTimeOptions(
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--collection-name")] string CollectionName,
[property: CliOption("--database-name")] string DatabaseName,
[property: CliOption("--location")] string Location,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;