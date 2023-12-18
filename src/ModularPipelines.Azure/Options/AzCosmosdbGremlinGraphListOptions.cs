using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cosmosdb", "gremlin", "graph", "list")]
public record AzCosmosdbGremlinGraphListOptions(
[property: CommandSwitch("--account-name")] int AccountName,
[property: CommandSwitch("--database-name")] string DatabaseName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;