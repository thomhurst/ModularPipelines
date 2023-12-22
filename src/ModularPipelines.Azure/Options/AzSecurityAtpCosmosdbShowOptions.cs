using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("security", "atp", "cosmosdb", "show")]
public record AzSecurityAtpCosmosdbShowOptions(
[property: CommandSwitch("--cosmosdb-account")] int CosmosdbAccount,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;