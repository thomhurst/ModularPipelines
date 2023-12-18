using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cosmosdb", "mongodb", "collection", "exists")]
public record AzCosmosdbMongodbCollectionExistsOptions(
[property: CommandSwitch("--account-name")] int AccountName,
[property: CommandSwitch("--database-name")] string DatabaseName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;