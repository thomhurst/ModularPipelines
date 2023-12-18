using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cosmosdb", "mongodb", "retrieve-latest-backup-time")]
public record AzCosmosdbMongodbRetrieveLatestBackupTimeOptions(
[property: CommandSwitch("--account-name")] int AccountName,
[property: CommandSwitch("--collection-name")] string CollectionName,
[property: CommandSwitch("--database-name")] string DatabaseName,
[property: CommandSwitch("--location")] string Location,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;