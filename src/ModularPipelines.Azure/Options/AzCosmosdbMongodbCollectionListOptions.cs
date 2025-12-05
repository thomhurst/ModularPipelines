using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("cosmosdb", "mongodb", "collection", "list")]
public record AzCosmosdbMongodbCollectionListOptions(
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--database-name")] string DatabaseName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;