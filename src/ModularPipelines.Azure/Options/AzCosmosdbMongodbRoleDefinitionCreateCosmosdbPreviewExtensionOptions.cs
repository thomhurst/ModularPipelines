using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("cosmosdb", "mongodb", "role", "definition", "create", "(cosmosdb-preview", "extension)")]
public record AzCosmosdbMongodbRoleDefinitionCreateCosmosdbPreviewExtensionOptions(
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--body")] string Body,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;