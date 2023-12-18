using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cosmosdb", "table", "restore")]
public record AzCosmosdbTableRestoreOptions(
[property: CommandSwitch("--account-name")] int AccountName,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--restore-timestamp")] string RestoreTimestamp,
[property: CommandSwitch("--table-name")] string TableName
) : AzOptions;