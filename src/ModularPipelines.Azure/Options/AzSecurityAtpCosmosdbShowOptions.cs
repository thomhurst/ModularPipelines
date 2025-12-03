using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("security", "atp", "cosmosdb", "show")]
public record AzSecurityAtpCosmosdbShowOptions(
[property: CliOption("--cosmosdb-account")] int CosmosdbAccount,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;