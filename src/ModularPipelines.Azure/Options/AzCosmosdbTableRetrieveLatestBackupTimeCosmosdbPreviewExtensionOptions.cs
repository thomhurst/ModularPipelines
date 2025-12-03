using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cosmosdb", "table", "retrieve-latest-backup-time", "(cosmosdb-preview", "extension)")]
public record AzCosmosdbTableRetrieveLatestBackupTimeCosmosdbPreviewExtensionOptions(
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--location")] string Location,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--table-name")] string TableName
) : AzOptions;