using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cosmosdb", "gremlin", "retrieve-latest-backup-time", "(cosmosdb-preview", "extension)")]
public record AzCosmosdbGremlinRetrieveLatestBackupTimeCosmosdbPreviewExtensionOptions(
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--database-name")] string DatabaseName,
[property: CliOption("--graph-name")] string GraphName,
[property: CliOption("--location")] string Location,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;