using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cosmosdb", "table", "retrieve-latest-backup-time")]
public record AzCosmosdbTableRetrieveLatestBackupTimeOptions(
[property: CommandSwitch("--account-name")] int AccountName,
[property: CommandSwitch("--location")] string Location,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--table-name")] string TableName
) : AzOptions;