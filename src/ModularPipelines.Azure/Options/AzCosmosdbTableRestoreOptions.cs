using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cosmosdb", "table", "restore")]
public record AzCosmosdbTableRestoreOptions(
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--restore-timestamp")] string RestoreTimestamp,
[property: CliOption("--table-name")] string TableName
) : AzOptions;