using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("security", "atp", "cosmosdb", "show")]
public record AzSecurityAtpCosmosdbShowOptions(
[property: CommandSwitch("--cosmosdb-account")] int CosmosdbAccount,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
}

