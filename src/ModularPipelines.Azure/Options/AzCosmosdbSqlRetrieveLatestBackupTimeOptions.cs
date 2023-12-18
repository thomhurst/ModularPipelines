using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cosmosdb", "sql", "retrieve-latest-backup-time")]
public record AzCosmosdbSqlRetrieveLatestBackupTimeOptions(
[property: CommandSwitch("--account-name")] int AccountName,
[property: CommandSwitch("--container-name")] string ContainerName,
[property: CommandSwitch("--database-name")] string DatabaseName,
[property: CommandSwitch("--location")] string Location,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;