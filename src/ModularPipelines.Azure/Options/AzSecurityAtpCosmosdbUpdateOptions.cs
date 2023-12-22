using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("security", "atp", "cosmosdb", "update")]
public record AzSecurityAtpCosmosdbUpdateOptions(
[property: CommandSwitch("--cosmosdb-account")] int CosmosdbAccount,
[property: BooleanCommandSwitch("--is-enabled")] bool IsEnabled,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;