using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cosmosdb", "sql", "role", "definition", "show")]
public record AzCosmosdbSqlRoleDefinitionShowOptions(
[property: CommandSwitch("--account-name")] int AccountName,
[property: CommandSwitch("--id")] string Id,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;