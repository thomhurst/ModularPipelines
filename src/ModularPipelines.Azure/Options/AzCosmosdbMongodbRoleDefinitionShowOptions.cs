using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cosmosdb", "mongodb", "role", "definition", "show")]
public record AzCosmosdbMongodbRoleDefinitionShowOptions(
[property: CommandSwitch("--account-name")] int AccountName,
[property: CommandSwitch("--id")] string Id,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
}