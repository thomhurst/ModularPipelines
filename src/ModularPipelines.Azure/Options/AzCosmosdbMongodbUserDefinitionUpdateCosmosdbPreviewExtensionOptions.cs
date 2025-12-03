using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cosmosdb", "mongodb", "user", "definition", "update", "(cosmosdb-preview", "extension)")]
public record AzCosmosdbMongodbUserDefinitionUpdateCosmosdbPreviewExtensionOptions(
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--body")] string Body,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;