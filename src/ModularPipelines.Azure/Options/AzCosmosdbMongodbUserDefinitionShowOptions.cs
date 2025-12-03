using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cosmosdb", "mongodb", "user", "definition", "show")]
public record AzCosmosdbMongodbUserDefinitionShowOptions(
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--id")] string Id,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;