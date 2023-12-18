using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cosmosdb", "mongodb", "role", "definition", "exists")]
public record AzCosmosdbMongodbRoleDefinitionExistsOptions(
[property: CommandSwitch("--account-name")] int AccountName,
[property: CommandSwitch("--id")] string Id,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
}