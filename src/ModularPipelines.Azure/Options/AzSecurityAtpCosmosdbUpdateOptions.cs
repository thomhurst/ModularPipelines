using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("security", "atp", "cosmosdb", "update")]
public record AzSecurityAtpCosmosdbUpdateOptions(
[property: CliOption("--cosmosdb-account")] int CosmosdbAccount,
[property: CliFlag("--is-enabled")] bool IsEnabled,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;