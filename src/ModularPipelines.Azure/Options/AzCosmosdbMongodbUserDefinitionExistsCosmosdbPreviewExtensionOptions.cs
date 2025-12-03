using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cosmosdb", "mongodb", "user", "definition", "exists", "(cosmosdb-preview", "extension)")]
public record AzCosmosdbMongodbUserDefinitionExistsCosmosdbPreviewExtensionOptions(
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--id")] string Id,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;