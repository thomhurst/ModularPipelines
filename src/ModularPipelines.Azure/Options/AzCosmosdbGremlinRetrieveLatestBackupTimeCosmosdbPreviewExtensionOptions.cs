using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cosmosdb", "gremlin", "retrieve-latest-backup-time", "(cosmosdb-preview", "extension)")]
public record AzCosmosdbGremlinRetrieveLatestBackupTimeCosmosdbPreviewExtensionOptions(
[property: CommandSwitch("--account-name")] int AccountName,
[property: CommandSwitch("--database-name")] string DatabaseName,
[property: CommandSwitch("--graph-name")] string GraphName,
[property: CommandSwitch("--location")] string Location,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;