using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("cosmosdb", "mongodb", "role", "definition", "exists")]
public record AzCosmosdbMongodbRoleDefinitionExistsOptions(
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--id")] string Id,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;