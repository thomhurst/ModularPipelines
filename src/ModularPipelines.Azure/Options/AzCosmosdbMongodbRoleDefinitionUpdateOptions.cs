using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cosmosdb", "mongodb", "role", "definition", "update")]
public record AzCosmosdbMongodbRoleDefinitionUpdateOptions(
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--body")] string Body,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;