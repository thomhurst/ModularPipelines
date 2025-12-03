using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cosmosdb", "service", "list", "(cosmosdb-preview", "extension)")]
public record AzCosmosdbServiceListCosmosdbPreviewExtensionOptions(
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--resource-group-name")] string ResourceGroupName
) : AzOptions;